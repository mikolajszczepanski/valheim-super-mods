using HarmonyLib;
using TMPro;
using UnityEngine;
using Valheim.UI;

namespace UnlimitedCarryWeight
{
    // The game's displays convert the maximum to an integer, which cannot represent infinity.
    // Format the weight here so the UI keeps showing real item weights without an overload warning.
    internal static class CarryWeightDisplayPatches
    {
        private static int GetDisplayedWeight(Player player)
        {
            return Mathf.CeilToInt(player.GetInventory().GetTotalWeight());
        }

        [HarmonyPatch(typeof(InventoryGui), "UpdateInventoryWeight", new[] { typeof(Player) })]
        private static class InventoryWeightPatch
        {
            private static bool Prefix(Player __0, TMP_Text ___m_weight)
            {
                ___m_weight.text = $"{GetDisplayedWeight(__0)}/\u221e";
                return false;
            }
        }

        [HarmonyPatch(typeof(RadialInventoryInfo), "MakeInventoryWeightString", new[] { typeof(Player) })]
        private static class RadialInventoryWeightPatch
        {
            private static bool Prefix(Player __0, ref string __result)
            {
                __result = $"{GetDisplayedWeight(__0)} / \u221e";
                return false;
            }
        }

        [HarmonyPatch(typeof(ThrowElement), "TotalWeightString", MethodType.Getter)]
        private static class ThrowWeightPatch
        {
            private static bool Prefix(ThrowElement __instance, ItemDrop.ItemData ___m_data, ref string __result)
            {
                var player = Player.m_localPlayer;
                if (player == null)
                {
                    __result = string.Empty;
                    return false;
                }

                var thrownWeight = Mathf.CeilToInt(__instance.ThrowAmount * ___m_data.GetNonStackedWeight());
                __result = $"{GetDisplayedWeight(player)} - {thrownWeight} / \u221e";
                return false;
            }
        }
    }
}
