using BepInEx;
using HarmonyLib;

namespace UnlimitedStamina
{
    [BepInPlugin(PluginId, "Unlimited Stamina", "1.0.0")]
    public sealed class Plugin : BaseUnityPlugin
    {
        private const string PluginId = "com.valheimmods.unlimitedstamina";

        private Harmony _harmony;

        private void Awake()
        {
            _harmony = new Harmony(PluginId);
            _harmony.PatchAll(typeof(Plugin).Assembly);
            Logger.LogInfo("Unlimited Stamina loaded.");
        }

        private void OnDestroy()
        {
            _harmony?.UnpatchSelf();
        }

        [HarmonyPatch(typeof(Player), nameof(Player.UseStamina))]
        private static class UseStaminaPatch
        {
            private static bool Prefix()
            {
                return false;
            }
        }

        [HarmonyPatch(typeof(Player), nameof(Player.HaveStamina))]
        private static class HaveStaminaPatch
        {
            private static bool Prefix(ref bool __result)
            {
                __result = true;
                return false;
            }
        }

        [HarmonyPatch(typeof(Player), "UpdateStats", new[] { typeof(float) })]
        private static class UpdateStatsPatch
        {
            private static void Postfix(Player __instance)
            {
                // Continuous actions can drain stamina directly during the stats update.
                __instance.AddStamina(__instance.GetMaxStamina());
            }
        }

        [HarmonyPatch(typeof(Player), nameof(Player.OnSpawned))]
        private static class OnSpawnedPatch
        {
            private static void Postfix(Player __instance)
            {
                __instance.AddStamina(__instance.GetMaxStamina());
            }
        }
    }
}
