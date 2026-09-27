# Expanded Item Stacks

This BepInEx plugin multiplies the original limit of every stackable item by 20: 50 becomes 1,000, 30 becomes 600, and 20 becomes 400. Single-item equipment stays at one per slot. Stack limits are capped at 65,535 because Valheim saves stack counts as unsigned 16-bit values.

Keep this plugin enabled while any inventory or container holds stacks larger than the normal item limit, and back up saves before disabling it.

## Install

Install this package with Gale, r2modman, or Thunderstore Mod Manager. The manager installs its Valheim BepInEx pack dependency. Launch Valheim with **Start modded**.

### Manual installation

Install the [Valheim BepInEx pack](https://thunderstore.io/c/valheim/p/denikson/BepInExPack_Valheim/) first. Download this package's ZIP, extract it, and place the extracted folder under the active Valheim profile's `BepInEx/plugins` directory. Keep `ExpandedItemStacks.dll` inside that folder; `manifest.json`, `README.md`, and `icon.png` can stay there too. Remove any older copy of `ExpandedItemStacks.dll` elsewhere in `BepInEx/plugins`, then launch Valheim modded.

For multiplayer, players using expanded stacks should install this plugin in their Gale profiles.

## Build from source

Run `dotnet build .\ValheimMods\ValheimMods.sln` from the repository root. The shared build target copies the plugin DLL to the selected Gale profile.

## License

GNU GPL v3.0 only (`GPL-3.0-only`). See the [repository license](https://github.com/mikolajszczepanski/valheim-super-mods/blob/main/LICENSE).
