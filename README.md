# Valheim Super Mods

Small mods for [Valheim](https://www.valheimgame.com/) that change things like inventory space, stamina, and what happens when you die. See the list below to find out what each mod does.

Want to install one? Find the published mods on [Thunderstore](https://thunderstore.io/c/valheim/p/ValheimSuperMods/). If you have a problem with a mod or an idea for one, [report it on GitHub Issues](https://github.com/mikolajszczepanski/valheim-super-mods/issues).

This repository contains the source code and build instructions for people who want to work on the mods.

## Plugins

| Plugin | What it does |
| --- | --- |
| [Increased Carry Weight](ValheimMods/IncreasedCarryWeight/README.md) | Multiplies a player's base carry weight by 10 (normally 300 to 3,000). |
| [Expanded Player Inventory](ValheimMods/ExpandedPlayerInventory/README.md) | Gives the player at least 64 inventory slots in an 8 × 8 grid. Existing larger inventories are preserved. |
| [Keep Inventory On Death](ValheimMods/KeepInventoryOnDeath/README.md) | Keeps all inventory items, including equipped items, when a player dies. |
| [Dismantle Equipment](ValheimMods/DismantleEquipment/README.md) | Shift + right-click a crafted inventory item to reclaim its recipe materials at your feet. |
| [Expanded Item Stacks](ValheimMods/ExpandedItemStacks/README.md) | Multiplies stackable item limits by 20, up to 65,535. |
| [Expanded Placeable Chests](ValheimMods/ExpandedPlaceableChests/README.md) | Triples the slots in player-built chests by adding rows. |
| [DevTools](ValheimMods/DevTools/README.md) | Adds the `spawn60` cheat command for testing. |
| [Hello Valheim](ValheimMods/HelloValheim/README.md) | A minimal starter plugin that writes `Hello Valheim is loaded!` to the BepInEx log. It is useful for checking that your build and modded launch work. |
| [Unlimited Stamina](ValheimMods/UnlimitedStamina/README.md) | Gives players unlimited stamina for movement and stamina-based actions. |

**Inventory note:** Expanded Player Inventory saves the extra inventory rows with the character. Removing the plugin does not shrink the inventory back to 32 slots. For multiplayer, each player who wants the extra rows should install the plugin in their own profile.

## Local development requirements

- Valheim installed on Windows.
- A .NET SDK or Visual Studio with C# development tools to build the solution.
- [Gale mod manager](https://github.com/Kesomannen/gale) with the [Valheim BepInEx pack](https://thunderstore.io/c/valheim/p/denikson/BepInExPack_Valheim/) installed in the Valheim profile you use. BepInEx through Gale is required to build and run these plugins. Start the game with **Start modded** in Gale.

Before building locally, remove any mod-manager-installed copies of these plugins from the Gale profile you will use for testing. Local builds copy plugin DLLs into that profile, and duplicate copies can conflict. Keep the BepInEx pack installed.

The projects expect Valheim at `C:\Program Files (x86)\Steam\steamapps\common\Valheim` and Gale's Valheim `Default` profile at `%APPDATA%\com.kesomannen.gale\valheim\profiles\Default`. You can override either path when building.

## Build and use

From the repository root, run:

```powershell
dotnet build .\ValheimMods\ValheimMods.sln
```

After a successful build, the shared [build target](ValheimMods/Directory.Build.targets) copies **all DLLs in each plugin project's output folder** to the selected Gale profile's `BepInEx\plugins` folder. With the default profile, that destination is:

```text
%APPDATA%\com.kesomannen.gale\valheim\profiles\Default\BepInEx\plugins
```

Launch Valheim with **Start modded** in Gale. To use only one plugin, build its `.csproj` and remove any other plugin DLLs you do not want from the Gale profile; building one project does not remove DLLs copied by an earlier build.

If your paths differ, pass MSBuild properties, for example:

```powershell
dotnet build .\ValheimMods\ValheimMods.sln -p:GaleProfile="C:\path\to\your\Gale\profile" -p:ValheimInstall="D:\SteamLibrary\steamapps\common\Valheim"
```

You can set `ValheimPluginsPath` to choose a different `BepInEx\plugins` destination. See each plugin's README for details and limitations.

## Package for Thunderstore and Gale

Run `powershell -NoProfile -ExecutionPolicy Bypass -File .\package.ps1` from the repository root. This builds the solution in Release configuration and writes one Thunderstore-compatible ZIP per plugin to `dist/`. Each archive contains that plugin's DLL, manifest, README, the shared 256 × 256 icon, and the license. The build still deploys DLLs to the selected Gale profile. Pass `-SkipBuild` to package an existing Release build, or `-Plugin UnlimitedStamina` to package one plugin.

To publish locally, install `tcli` with `dotnet tool install tcli --tool-path .tools --version 0.2.4`, create a Thunderstore team service-account token, and save it outside the repository in `%USERPROFILE%\.thunderstore_token`. Run `powershell -NoProfile -ExecutionPolicy Bypass -File .\publish-thunderstore.ps1` to prepare and inspect publisher settings, then add `-Publish` to upload. Use `-Plugin UnlimitedStamina` to upload one plugin. Package versions must match the corresponding `BepInPlugin` versions; `package.ps1` verifies this. The ZIPs, local publisher configuration, and tool installation are ignored by Git.

## Repository layout

```text
ValheimMods/
  ValheimMods.sln                 Visual Studio solution
  Directory.Build.props          Default Gale profile and deployment path
  Directory.Build.targets        Copies plugin DLLs after a build
  IncreasedCarryWeight/          Player carry weight plugin
  ExpandedPlayerInventory/       Player inventory row plugin
  KeepInventoryOnDeath/          Retains inventory after death
  DismantleEquipment/            Recovers materials from crafted items
  ExpandedItemStacks/            Item stack limit plugin
  ExpandedPlaceableChests/       Player-built chest slot plugin
  DevTools/                      Developer console commands
  HelloValheim/                  Minimal logging plugin
  UnlimitedStamina/              Unlimited player stamina plugin
```

Contributions and issue reports are welcome. Keep new plugins in separate projects and use the shared build settings so their DLLs deploy to the selected Gale profile.

## License

This repository is licensed under the [GNU General Public License v3.0 only](LICENSE) (`GPL-3.0-only`). Copyright (C) 2026 ValheimSuperMods contributors.
