# Expanded Player Inventory

This BepInEx plugin gives each player at least 8 inventory rows (64 slots with Valheim's standard 8 columns). Equipment still uses inventory slots. It preserves inventories that are already larger than 8 rows.

The extra rows are applied when a player spawns and saved with the character. Removing the plugin does not automatically shrink the inventory back to 32 slots.

## Build and install

From the repository root, run `dotnet build .\ValheimMods\ValheimMods.sln`. The shared build target copies `ExpandedPlayerInventory.dll` to the selected Gale profile's `BepInEx\plugins` folder. Launch Valheim with **Start modded** in Gale.

For multiplayer, each player who wants the extra inventory rows should install this plugin in their own Gale profile.
