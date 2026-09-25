using BepInEx;
using HarmonyLib;

namespace ExtendedPlayerCapacity
{
    [BepInPlugin(PluginId, "Extended Player Capacity", "1.0.0")]
    public sealed class Plugin : BaseUnityPlugin
    {
        private const string PluginId = "com.valheimmods.extendedplayercapacity";
        private const float CarryWeightMultiplier = 10f;
        private const int InventoryRows = 8;

        private Harmony _harmony;

        private void Awake()
        {
            _harmony = new Harmony(PluginId);
            _harmony.PatchAll(typeof(Plugin).Assembly);
            Logger.LogInfo("Extended Player Capacity loaded: 10x base carry weight and 64 inventory slots.");
        }

        private void OnDestroy()
        {
            _harmony?.UnpatchSelf();
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
    }
}
