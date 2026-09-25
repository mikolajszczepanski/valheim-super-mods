# Unlimited Stamina

This BepInEx plugin gives players unlimited stamina. Running, swimming, sneaking, dodging, attacking, and other stamina-based actions remain available without depleting the stamina bar. It refills stamina when the player spawns and after each stats update, including drains from continuous actions.

## Build and install

From the repository root, run `dotnet build .\ValheimMods\ValheimMods.sln`. The shared build target copies `UnlimitedStamina.dll` to the selected Gale profile's `BepInEx\plugins` folder. Launch Valheim with **Start modded** in Gale.

For multiplayer, each player who wants unlimited stamina should install this plugin in their own Gale profile. The plugin changes player stamina only; it does not change eitr or health.
