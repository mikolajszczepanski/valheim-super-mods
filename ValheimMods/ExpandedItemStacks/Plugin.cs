using System;
using System.Collections.Generic;
using BepInEx;
using HarmonyLib;

namespace ExpandedItemStacks
{
    [BepInPlugin(PluginId, "Expanded Item Stacks", "1.0.0")]
    public sealed class Plugin : BaseUnityPlugin
    {
        private const string PluginId = "com.valheimmods.expandeditemstacks";
        private const int StackMultiplier = 20;

        private static readonly Dictionary<string, int> ExpandedStackLimits =
            new Dictionary<string, int>(StringComparer.Ordinal);

        private Harmony _harmony;

        private void Awake()
        {
            _harmony = new Harmony(PluginId);
            _harmony.PatchAll(typeof(Plugin).Assembly);
            Logger.LogInfo("Expanded Item Stacks loaded: 20x stack limits for stackable items.");
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
    }
}
