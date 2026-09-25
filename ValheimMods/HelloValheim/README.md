# Hello Valheim

A minimal BepInEx 5 plugin. When Valheim loads it, the plugin writes `Hello Valheim is loaded!` to the BepInEx log. The project references the BepInEx installation in Gale's `Default` Valheim profile.

## Requirement

Install the [Valheim BepInEx pack](https://thunderstore.io/c/valheim/p/denikson/BepInExPack_Valheim/) in your Valheim profile through [Gale](https://github.com/Kesomannen/gale). BepInEx in that Gale profile is required to build and run this plugin. Launch Valheim with **Start modded** in Gale so BepInEx loads the plugin.

## Build

1. From the solution folder, run:

   ```powershell
   dotnet build .\ValheimMods.sln
   ```

   The build copies every DLL in the plugin project's output folder to Gale's Valheim `Default` profile at `%APPDATA%\com.kesomannen.gale\valheim\profiles\Default\BepInEx\plugins`.

   If Valheim is installed elsewhere, pass `-p:ValheimInstall="C:\path\to\Valheim"`. If your Gale profile is not `Default`, pass `-p:GaleProfile="C:\path\to\your\Gale\profile"`. To choose another plugin folder directly, pass `-p:ValheimPluginsPath="C:\path\to\BepInEx\plugins"`.

2. Launch Valheim with **Start modded** in Gale and check Gale's BepInEx log for the message.

The build copies DLLs from this project's output folder. BepInEx and Unity runtime assemblies are supplied by Gale's profile and the game; the project does not copy those assemblies.
