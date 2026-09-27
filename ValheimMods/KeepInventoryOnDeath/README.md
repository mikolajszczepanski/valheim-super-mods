# Keep Inventory On Death

This BepInEx plugin keeps every item in the player's inventory when they die, including equipped items. It prevents the game from creating a tombstone and moving items into it. The normal death and respawn sequence still runs, including the world's skill loss rules.

## Install

Install this package with Gale, r2modman, or Thunderstore Mod Manager. The manager installs its Valheim BepInEx pack dependency. Launch Valheim with **Start modded**.

### Manual installation

Install the [Valheim BepInEx pack](https://thunderstore.io/c/valheim/p/denikson/BepInExPack_Valheim/) first. Download this package's ZIP, extract it, and place the extracted folder under the active Valheim profile's `BepInEx/plugins` directory. Keep `KeepInventoryOnDeath.dll` inside that folder; `manifest.json`, `README.md`, and `icon.png` can stay there too. Remove any older copy of `KeepInventoryOnDeath.dll` elsewhere in `BepInEx/plugins`, then launch Valheim modded.

For multiplayer, each player who wants to keep their inventory should install this plugin in their own Gale profile. It does not restore items lost before the plugin was installed.

## Build from source

Run `dotnet build .\ValheimMods\ValheimMods.sln` from the repository root. The shared build target copies the plugin DLL to the selected Gale profile.

## License

GNU GPL v3.0 only (`GPL-3.0-only`). See the [repository license](https://github.com/mikolajszczepanski/valheim-super-mods/blob/main/LICENSE).
