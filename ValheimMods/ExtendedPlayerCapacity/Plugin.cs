using BepInEx;
using HarmonyLib;

namespace ExtendedPlayerCapacity
{
    [BepInPlugin(PluginId, "Extended Player Capacity", "1.2.0")]
    public sealed class Plugin : BaseUnityPlugin
    {
        private const string PluginId = "com.valheimmods.extendedplayercapacity";
        private const float CarryWeightMultiplier = 10f;
        private const int InventoryRows = 8;
        private const int OriginalStackLimit = 50;
        private const int NewStackLimit = 1000;

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
            Logger.LogInfo("Extended Player Capacity loaded: 10x base carry weight, 64 inventory slots, and 1,000-item stacks for items normally capped at 50.");
        }

        private void OnDestroy()
        {
            _harmony?.UnpatchSelf();
        }

        private static void IncreaseStackLimit(ItemDrop itemDrop)
        {
            var shared = itemDrop?.m_itemData?.m_shared;
            if (shared != null && shared.m_maxStackSize == OriginalStackLimit)
            {
                shared.m_maxStackSize = NewStackLimit;
            }
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
                IncreaseStackLimit(__instance);
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
