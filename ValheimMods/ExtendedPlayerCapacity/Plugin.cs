using System;
using System.Collections.Generic;
using BepInEx;
using HarmonyLib;

namespace ExtendedPlayerCapacity
{
    [BepInPlugin(PluginId, "Extended Player Capacity", "1.3.0")]
    public sealed class Plugin : BaseUnityPlugin
    {
        private const string PluginId = "com.valheimmods.extendedplayercapacity";
        private const float CarryWeightMultiplier = 10f;
        private const int InventoryRows = 8;
        private const int StackMultiplier = 20;

        private static readonly Dictionary<string, int> ExpandedStackLimits =
            new Dictionary<string, int>(StringComparer.Ordinal);

        private static readonly string[] ItemPrefabs =
        {
            "Wood", "Stone", "Flint", "Resin", "LeatherScraps", "DeerHide",
            "Feathers", "Dandelion", "Honey", "BoneFragments", "GreydwarfEye",
            "FineWood", "RoundLog", "ElderBark", "Bronze", "Copper", "Tin",
            "Iron", "Silver", "BlackMetal", "Flametal", "YggdrasilWood",
            "Blackwood", "BlackMarble", "Grausten", "Coal", "SurtlingCore",
            "AncientSeed", "HardAntler", "DragonTear", "Crystal", "Obsidian",
            "FreezeGland", "WolfPelt", "WolfFang", "TrollHide", "Chitin",
            "SerpentScale", "Bloodbag", "Entrails", "Ooze", "Guck", "Root",
            "Tar", "LoxPelt", "Needle", "Barley", "Flax", "LinenThread",
            "IronNails", "BronzeNails", "Carapace", "Eitr", "Sap", "ScaleHide",
            "GemstoneBlue", "GemstoneGreen", "GemstoneRed", "Thunderstone", "Wisp"
        };

        private Harmony _harmony;

        private void Awake()
        {
            _harmony = new Harmony(PluginId);
            _harmony.PatchAll(typeof(Plugin).Assembly);
            Logger.LogInfo("Extended Player Capacity loaded: 10x base carry weight, 64 inventory slots, and 20x stack limits for stackable items.");
        }

        private void OnDestroy()
        {
            _harmony?.UnpatchSelf();
        }

        private static void IncreaseStackLimit(ItemDrop itemDrop)
        {
            var shared = itemDrop?.m_itemData?.m_shared;
            if (shared == null)
            {
                return;
            }

            string prefabName = itemDrop.gameObject.name;
            if (!ExpandedStackLimits.TryGetValue(prefabName, out int expandedLimit))
            {
                if (shared.m_maxStackSize <= 1)
                {
                    return;
                }

                // Inventory saves stack counts as UInt16. Remember the result
                // so registry refreshes never multiply the same item again.
                expandedLimit = (int)Math.Min((long)shared.m_maxStackSize * StackMultiplier, ushort.MaxValue);
                ExpandedStackLimits.Add(prefabName, expandedLimit);
            }

            shared.m_maxStackSize = expandedLimit;
        }

        [HarmonyPatch(typeof(ObjectDB), "UpdateRegisters")]
        private static class ObjectDbUpdateRegistersPatch
        {
            private static void Postfix(ObjectDB __instance)
            {
                foreach (var prefab in __instance.m_items)
                {
                    if (prefab != null)
                    {
                        IncreaseStackLimit(prefab.GetComponent<ItemDrop>());
                    }
                }
            }
        }

        [HarmonyPatch(typeof(ItemDrop), "Awake")]
        private static class ItemDropAwakePatch
        {
            private static void Postfix(ItemDrop __instance)
            {
                // A spawned drop may own a copy of SharedData. Copy the limit
                // from its registered prefab instead of multiplying it again.
                var prefab = __instance.m_itemData?.m_dropPrefab;
                var instanceShared = __instance.m_itemData?.m_shared;
                if (prefab != null && instanceShared != null)
                {
                    var prefabShared = prefab.GetComponent<ItemDrop>()?.m_itemData?.m_shared;
                    if (prefabShared != null)
                    {
                        instanceShared.m_maxStackSize = prefabShared.m_maxStackSize;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(Player), "Awake")]
        private static class PlayerAwakePatch
        {
            private static void Postfix(Player __instance)
            {
                __instance.m_maxCarryWeight *= CarryWeightMultiplier;
            }
        }

        [HarmonyPatch(typeof(Player), nameof(Player.OnSpawned))]
        private static class PlayerOnSpawnedPatch
        {
            private static void Postfix(Player __instance)
            {
                // Valheim's method also updates the UI and persists the row count.
                // Keep any larger inventory from another mod so its items are not dropped.
                if (__instance.GetInventory().GetHeight() < InventoryRows)
                {
                    __instance.SetInventorySize(InventoryRows);
                }
            }
        }

        [HarmonyPatch(typeof(Terminal), "InitTerminal")]
        private static class TerminalInitPatch
        {
            private static bool _registered;

            private static void Postfix()
            {
                if (_registered)
                {
                    return;
                }

                new Terminal.ConsoleCommand(
                    "spawn60",
                    "Spawns one each of 60 different materials",
                    SpawnSixtyItems,
                    isCheat: true);
                _registered = true;
            }

            private static void SpawnSixtyItems(Terminal.ConsoleEventArgs args)
            {
                foreach (string prefab in ItemPrefabs)
                {
                    // Use Valheim's own spawn command so its normal cheat checks and
                    // item handling apply to every spawned prefab.
                    args.Context.TryRunCommand("spawn " + prefab + " 1", false, false);
                }

                args.Context.AddString("Spawned 60 different items near your character.");
            }
        }
    }
}
