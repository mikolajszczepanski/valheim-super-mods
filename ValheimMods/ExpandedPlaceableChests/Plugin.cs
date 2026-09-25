using System;
using System.Collections.Generic;
using BepInEx;
using HarmonyLib;

namespace ExpandedPlaceableChests
{
    [BepInPlugin(PluginId, "Expanded Placeable Chests", "1.0.0")]
    public sealed class Plugin : BaseUnityPlugin
    {
        private const string PluginId = "com.valheimmods.expandedplaceablechests";
        private const int ChestSlotMultiplier = 3;

        private static readonly Dictionary<string, int> ExpandedChestHeights =
            new Dictionary<string, int>(StringComparer.Ordinal);

        private Harmony _harmony;

        private void Awake()
        {
            _harmony = new Harmony(PluginId);
            _harmony.PatchAll(typeof(Plugin).Assembly);
            Logger.LogInfo("Expanded Placeable Chests loaded: 3x slots in player-built chests.");
        }

        private void OnDestroy()
        {
            _harmony?.UnpatchSelf();
        }

        [HarmonyPatch(typeof(Container), "Awake")]
        private static class ChestAwakePatch
        {
            private static void Prefix(Container __instance)
            {
                string prefabName = __instance.gameObject.name;
                int cloneSuffix = prefabName.IndexOf("(Clone)", StringComparison.Ordinal);
                if (cloneSuffix >= 0)
                {
                    prefabName = prefabName.Substring(0, cloneSuffix);
                }

                // Only player-placeable chests; other containers keep their own sizes.
                if (!prefabName.StartsWith("piece_chest", StringComparison.Ordinal) ||
                    __instance.m_height <= 0)
                {
                    return;
                }

                if (!ExpandedChestHeights.TryGetValue(prefabName, out int expandedHeight))
                {
                    expandedHeight = __instance.m_height * ChestSlotMultiplier;
                    ExpandedChestHeights.Add(prefabName, expandedHeight);
                }

                // Awake creates the inventory from m_width and m_height. Remembering
                // the target prevents a prefab clone from being multiplied twice.
                if (__instance.m_height < expandedHeight)
                {
                    __instance.m_height = expandedHeight;
                }
            }
        }
    }
}
