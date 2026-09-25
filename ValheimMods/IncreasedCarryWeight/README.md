# Increased Carry Weight

This BepInEx plugin multiplies each player's base carry weight by 10 (normally 300 to 3,000). It does not change inventory slots or item stack limits.

## Build and install

From the repository root, run `dotnet build .\ValheimMods\ValheimMods.sln`. The shared build target copies `IncreasedCarryWeight.dll` to the selected Gale profile's `BepInEx\plugins` folder. Launch Valheim with **Start modded** in Gale.

For multiplayer, each player who wants the increased carry weight should install this plugin in their own Gale profile.
