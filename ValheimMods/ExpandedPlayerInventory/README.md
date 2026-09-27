# Expanded Player Inventory

This BepInEx plugin gives each player at least 8 inventory rows (64 slots with Valheim's standard 8 columns). Equipment still uses inventory slots. It preserves inventories that are already larger than 8 rows.

The extra rows are applied when a player spawns and saved with the character. Removing the plugin does not automatically shrink the inventory back to 32 slots.

## Install

Install this package with Gale, r2modman, or Thunderstore Mod Manager. The manager installs its Valheim BepInEx pack dependency. Launch Valheim with **Start modded**.

### Manual installation

Install the [Valheim BepInEx pack](https://thunderstore.io/c/valheim/p/denikson/BepInExPack_Valheim/) first. Download this package's ZIP, extract it, and place the extracted folder under the active Valheim profile's `BepInEx/plugins` directory. Keep `ExpandedPlayerInventory.dll` inside that folder; `manifest.json`, `README.md`, and `icon.png` can stay there too. Remove any older copy of `ExpandedPlayerInventory.dll` elsewhere in `BepInEx/plugins`, then launch Valheim modded.

For multiplayer, each player who wants the extra inventory rows should install this plugin in their own Gale profile.

## Build from source

Run `dotnet build .\ValheimMods\ValheimMods.sln` from the repository root. The shared build target copies the plugin DLL to the selected Gale profile.

## License

GNU GPL v3.0 only (`GPL-3.0-only`). See the [repository license](https://github.com/mikolajszczepanski/valheim-super-mods/blob/main/LICENSE).
