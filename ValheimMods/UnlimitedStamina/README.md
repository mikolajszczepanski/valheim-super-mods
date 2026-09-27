# Unlimited Stamina

This BepInEx plugin gives players unlimited stamina. Running, swimming, sneaking, dodging, attacking, and other stamina-based actions remain available without depleting the stamina bar. It refills stamina when the player spawns and after each stats update, including drains from continuous actions.

## Install

Install this package with Gale, r2modman, or Thunderstore Mod Manager. The manager installs its Valheim BepInEx pack dependency. Launch Valheim with **Start modded**.

### Manual installation

Install the [Valheim BepInEx pack](https://thunderstore.io/c/valheim/p/denikson/BepInExPack_Valheim/) first. Download this package's ZIP, extract it, and place the extracted folder under the active Valheim profile's `BepInEx/plugins` directory. Keep `UnlimitedStamina.dll` inside that folder; `manifest.json`, `README.md`, and `icon.png` can stay there too. Remove any older copy of `UnlimitedStamina.dll` elsewhere in `BepInEx/plugins`, then launch Valheim modded.

For multiplayer, each player who wants unlimited stamina should install this plugin in their own Gale profile. The plugin changes player stamina only; it does not change eitr or health.

## Build from source

Run `dotnet build .\ValheimMods\ValheimMods.sln` from the repository root. The shared build target copies the plugin DLL to the selected Gale profile.

## License

GNU GPL v3.0 only (`GPL-3.0-only`). See the [repository license](https://github.com/mikolajszczepanski/valheim-super-mods/blob/main/LICENSE).
