using BepInEx;
using HarmonyLib;

namespace UnlimitedCarryWeight
{
    [BepInPlugin(PluginId, "Unlimited Carry Weight", "1.0.0")]
    public sealed class Plugin : BaseUnityPlugin
    {
        private const string PluginId = "com.valheimmods.unlimitedcarryweight";

        private Harmony _harmony;

        private void Awake()
        {
            _harmony = new Harmony(PluginId);
            _harmony.PatchAll(typeof(Plugin).Assembly);
            Logger.LogInfo("Unlimited Carry Weight loaded.");
        }

        private void OnDestroy()
        {
            _harmony?.UnpatchSelf();
        }

        [HarmonyPatch(typeof(Player), nameof(Player.GetMaxCarryWeight))]
        private static class GetMaxCarryWeightPatch
        {
            private static void Postfix(ref float __result)
            {
                // Apply after equipment and world modifiers without changing saved player data.
                __result = float.PositiveInfinity;
            }
        }
    }
}
