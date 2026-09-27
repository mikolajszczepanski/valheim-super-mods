# Expanded Placeable Chests

This BepInEx plugin triples the number of slots in player-built chests. It adds rows while keeping each chest's original width, so existing item positions stay the same. It applies to wooden, reinforced, black metal, and other placeable chest variants, including existing chests when they load. The chest inventory can scroll to show the added rows. Carts, ships, graves, and loot containers keep their normal sizes.

Keep this plugin enabled while chest items occupy the added rows, and back up saves before disabling it.

## Install

Install this package with Gale, r2modman, or Thunderstore Mod Manager. The manager installs its Valheim BepInEx pack dependency. Launch Valheim with **Start modded**.

### Manual installation

Install the [Valheim BepInEx pack](https://thunderstore.io/c/valheim/p/denikson/BepInExPack_Valheim/) first. Download this package's ZIP, extract it, and place the extracted folder under the active Valheim profile's `BepInEx/plugins` directory. Keep `ExpandedPlaceableChests.dll` inside that folder; `manifest.json`, `README.md`, and `icon.png` can stay there too. Remove any older copy of `ExpandedPlaceableChests.dll` elsewhere in `BepInEx/plugins`, then launch Valheim modded.

For multiplayer, players accessing expanded chests should install this plugin in their Gale profiles.

## Build from source

Run `dotnet build .\ValheimMods\ValheimMods.sln` from the repository root. The shared build target copies the plugin DLL to the selected Gale profile.

## License

GNU GPL v3.0 only (`GPL-3.0-only`). See the [repository license](https://github.com/mikolajszczepanski/valheim-super-mods/blob/main/LICENSE).
