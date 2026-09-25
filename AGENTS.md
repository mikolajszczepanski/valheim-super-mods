# Valheim mod workspace

- This solution builds Valheim BepInEx plugins. Running them requires [Gale](https://github.com/Kesomannen/gale) with the [Valheim BepInEx pack](https://thunderstore.io/c/valheim/p/denikson/BepInExPack_Valheim/) installed in the selected Gale Valheim profile. The user's current installation is in the `Default` profile under `%APPDATA%\com.kesomannen.gale\valheim\profiles\Default`. Launch Valheim with **Start modded** in Gale to load the plugins.
- Each plugin project must set `<IsValheimPlugin>true</IsValheimPlugin>`. The shared `Directory.Build.targets` copies every `.dll` from that project's build output to the Gale profile's `BepInEx\plugins` folder after a successful build. Preserve this deployment behavior when adding or editing projects.
- The destination can be changed with the MSBuild property `ValheimPluginsPath`; the Gale profile can be changed with `GaleProfile`. Do not assume BepInEx is installed in the Steam game directory.
- Reference BepInEx from the Gale profile and Unity assemblies from the Valheim game directory. Do not copy BepInEx or Unity runtime assemblies into the plugins folder.
- When changing build or deployment behavior, run `dotnet build ValheimMods\ValheimMods.sln` and verify the expected plugin DLL exists in the selected profile's `BepInEx\plugins` folder.
