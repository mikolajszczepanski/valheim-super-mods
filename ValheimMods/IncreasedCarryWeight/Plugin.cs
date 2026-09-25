using BepInEx;
using HarmonyLib;

namespace IncreasedCarryWeight
{
    [BepInPlugin(PluginId, "Increased Carry Weight", "1.0.0")]
    public sealed class Plugin : BaseUnityPlugin
    {
        private const string PluginId = "com.valheimmods.increasedcarryweight";
        private const float CarryWeightMultiplier = 10f;

        private Harmony _harmony;

        private void Awake()
        {
            _harmony = new Harmony(PluginId);
            _harmony.PatchAll(typeof(Plugin).Assembly);
            Logger.LogInfo("Increased Carry Weight loaded: 10x base carry weight.");
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
    }
}
