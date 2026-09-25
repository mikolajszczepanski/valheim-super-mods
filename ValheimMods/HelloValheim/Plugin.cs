using BepInEx;

namespace HelloValheim
{
    [BepInPlugin("com.valheimmods.hellovalheim", "Hello Valheim", "1.0.0")]
    public sealed class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            Logger.LogInfo("Hello Valheim is loaded!");
        }
    }
}
