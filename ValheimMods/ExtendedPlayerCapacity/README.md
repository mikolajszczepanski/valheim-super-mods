# Extended Player Capacity

This BepInEx plugin gives each player 10 times the base carry weight (3,000 instead of 300) and at least 64 inventory slots (8 columns by 8 rows). Equipment still occupies inventory slots, as in Valheim's standard inventory.

The extra rows are applied when a player spawns and saved with the character. Removing the plugin does not automatically shrink a character's inventory back to 32 slots. An existing inventory larger than 64 slots is left alone to avoid dropping items.

## Build and install

From the `ValheimMods` solution folder, run `dotnet build .\ValheimMods.sln`. The shared build target copies `ExtendedPlayerCapacity.dll` to the Gale `Default` profile's `BepInEx\plugins` folder. Launch Valheim using Gale's **Start modded** option.

For multiplayer, each player who wants the increased capacity should install the plugin in their own Gale profile.
