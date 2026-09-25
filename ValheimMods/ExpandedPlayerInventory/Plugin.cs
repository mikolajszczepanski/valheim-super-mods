using BepInEx;
using HarmonyLib;

namespace ExpandedPlayerInventory
{
    [BepInPlugin(PluginId, "Expanded Player Inventory", "1.0.0")]
    public sealed class Plugin : BaseUnityPlugin
    {
        private const string PluginId = "com.valheimmods.expandedplayerinventory";
        private const int InventoryRows = 8;

        private Harmony _harmony;

        private void Awake()
        {
            _harmony = new Harmony(PluginId);
            _harmony.PatchAll(typeof(Plugin).Assembly);
            Logger.LogInfo("Expanded Player Inventory loaded: at least 8 inventory rows.");
        }

        private void OnDestroy()
        {
            _harmony?.UnpatchSelf();
        }

        [HarmonyPatch(typeof(Player), nameof(Player.OnSpawned))]
        private static class PlayerOnSpawnedPatch
        {
            private static void Postfix(Player __instance)
            {
                // SetInventorySize also updates the UI and persists the row count.
                // Preserve a larger inventory so its items are not dropped.
                if (__instance.GetInventory().GetHeight() < InventoryRows)
                {
                    __instance.SetInventorySize(InventoryRows);
                }
            }
        }
    }
}
