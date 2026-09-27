# Hello Valheim

A minimal BepInEx 5 plugin. When Valheim loads it, the plugin writes `Hello Valheim is loaded!` to the BepInEx log.

## Install

Install this package with Gale, r2modman, or Thunderstore Mod Manager. The manager installs its Valheim BepInEx pack dependency. Launch Valheim with **Start modded**.

### Manual installation

Install the [Valheim BepInEx pack](https://thunderstore.io/c/valheim/p/denikson/BepInExPack_Valheim/) first. Download this package's ZIP, extract it, and place the extracted folder under the active Valheim profile's `BepInEx/plugins` directory. Keep `HelloValheim.dll` inside that folder; `manifest.json`, `README.md`, and `icon.png` can stay there too. Remove any older copy of `HelloValheim.dll` elsewhere in `BepInEx/plugins`, then launch Valheim modded.

## Build from source

1. From the solution folder, run:

   ```powershell
   dotnet build .\ValheimMods.sln
   ```

   The build copies every DLL in the plugin project's output folder to the selected Gale profile's `BepInEx\plugins` folder. By default, this is `%APPDATA%\com.kesomannen.gale\valheim\profiles\Default\BepInEx\plugins`.

   If Valheim is installed elsewhere, pass `-p:ValheimInstall="C:\path\to\Valheim"`. If your Gale profile is not `Default`, pass `-p:GaleProfile="C:\path\to\your\Gale\profile"`. To choose another plugin folder directly, pass `-p:ValheimPluginsPath="C:\path\to\BepInEx\plugins"`.

2. Launch Valheim with **Start modded** in Gale and check Gale's BepInEx log for the message.

The build copies DLLs from this project's output folder. BepInEx and Unity runtime assemblies are supplied by Gale's profile and the game; the project does not copy those assemblies.

## License

GNU GPL v3.0 only (`GPL-3.0-only`). See the [repository license](https://github.com/mikolajszczepanski/valheim-super-mods/blob/main/LICENSE).
