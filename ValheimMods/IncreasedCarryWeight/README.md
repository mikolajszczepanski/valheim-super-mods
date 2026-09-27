# Increased Carry Weight

This BepInEx plugin multiplies each player's base carry weight by 10 (normally 300 to 3,000). It does not change inventory slots or item stack limits.

## Install

Install this package with Gale, r2modman, or Thunderstore Mod Manager. The manager installs its Valheim BepInEx pack dependency. Launch Valheim with **Start modded**.

### Manual installation

Install the [Valheim BepInEx pack](https://thunderstore.io/c/valheim/p/denikson/BepInExPack_Valheim/) first. Download this package's ZIP, extract it, and place the extracted folder under the active Valheim profile's `BepInEx/plugins` directory. Keep `IncreasedCarryWeight.dll` inside that folder; `manifest.json`, `README.md`, and `icon.png` can stay there too. Remove any older copy of `IncreasedCarryWeight.dll` elsewhere in `BepInEx/plugins`, then launch Valheim modded.

For multiplayer, each player who wants the increased carry weight should install this plugin in their own Gale profile.

## Build from source

Run `dotnet build .\ValheimMods\ValheimMods.sln` from the repository root. The shared build target copies the plugin DLL to the selected Gale profile.

## License

GNU GPL v3.0 only (`GPL-3.0-only`). See the [repository license](https://github.com/mikolajszczepanski/valheim-super-mods/blob/main/LICENSE).
