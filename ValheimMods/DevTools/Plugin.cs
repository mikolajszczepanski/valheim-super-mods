using BepInEx;
using HarmonyLib;

namespace DevTools
{
    [BepInPlugin(PluginId, "Dev Tools", "1.0.0")]
    public sealed class Plugin : BaseUnityPlugin
    {
        private const string PluginId = "com.valheimmods.devtools";

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
            Logger.LogInfo("Dev Tools loaded: spawn60 console command available.");
        }

        private void OnDestroy()
        {
            _harmony?.UnpatchSelf();
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
                    // Use Valheim's own spawn command for normal cheat checks and item handling.
                    args.Context.TryRunCommand("spawn " + prefab + " 1", false, false);
                }

                args.Context.AddString("Spawned 60 different items near your character.");
            }
        }
    }
}
