using BepInEx;
using HarmonyLib;

namespace KeepInventoryOnDeath
{
    [BepInPlugin(PluginId, "Keep Inventory On Death", "1.0.0")]
    public sealed class Plugin : BaseUnityPlugin
    {
        private const string PluginId = "com.valheimmods.keepinventoryondeath";

        private Harmony _harmony;

        private void Awake()
        {
            _harmony = new Harmony(PluginId);
            _harmony.PatchAll(typeof(Plugin).Assembly);
            Logger.LogInfo("Keep Inventory On Death loaded.");
        }

        private void OnDestroy()
        {
            _harmony?.UnpatchSelf();
        }

        [HarmonyPatch(typeof(Player), nameof(Player.CreateTombStone))]
        private static class CreateTombStonePatch
        {
            private static bool Prefix()
            {
                // This method moves the player's items into a grave. Skipping it
                // leaves every inventory slot, including equipped items, intact.
                return false;
            }
        }
    }
}
