# Valheim Super Mods

Small mods for [Valheim](https://www.valheimgame.com/) that change things like inventory space, stamina, and what happens when you die. See the list below to find out what each mod does.

Want to install one? Find the published mods on [Thunderstore](https://thunderstore.io/c/valheim/p/ValheimSuperMods/). If you have a problem with a mod or an idea for one, [report it on GitHub Issues](https://github.com/mikolajszczepanski/valheim-super-mods/issues).

This repository contains the source code and build instructions for people who want to work on the mods.

## Plugins

| Plugin | What it does |
| --- | --- |
| [Increased Carry Weight](ValheimMods/IncreasedCarryWeight/README.md) | Multiplies a player's base carry weight by 10 (normally 300 to 3,000). |
| [Unlimited Carry Weight](ValheimMods/UnlimitedCarryWeight/README.md) | Removes the player carry weight limit while preserving item weights and inventory slots. |
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

Before copying a local build into your testing profile, remove any mod-manager-installed duplicate of that plugin. Duplicate copies can conflict. Keep the BepInEx pack installed. Building alone does not change your profile.

The projects expect Valheim at `C:\Program Files (x86)\Steam\steamapps\common\Valheim` and Gale's Valheim `Default` profile at `%APPDATA%\com.kesomannen.gale\valheim\profiles\Default`. You can override either path when building.

## Build and use

From the repository root, run:

```powershell
dotnet build .\ValheimMods\ValheimMods.sln
```

Building copies nothing to your local mods folder by default. After building, explicitly select the mods you want to copy. For example, copy only Unlimited Carry Weight from the existing Debug build:

```powershell
.\copy-mods.ps1 -Plugin UnlimitedCarryWeight
```

To copy several mods, pass their project names as a PowerShell array, for example `-Plugin UnlimitedCarryWeight,UnlimitedStamina`. Use `-Configuration Release` for an existing Release build. The script checks the requested outputs, then uses the shared [copy target](ValheimMods/Directory.Build.targets) to copy DLLs from only those projects. It does not rebuild or remove other installed mods. Copying prints the full destination path. With the default profile, that destination is:

```text
%APPDATA%\com.kesomannen.gale\valheim\profiles\Default\BepInEx\plugins
```

Launch Valheim with **Start modded** in Gale after copying the mods you selected. When working with an agent, it must ask which mods to copy after a build unless you already explicitly selected them, and show the local folder path after copying.

For an explicitly requested copy during a build, select the project by name:

```powershell
dotnet build .\ValheimMods\ValheimMods.sln -p:ValheimPluginsToCopy=UnlimitedCarryWeight
```

Only the named project copies its output DLLs. For multiple selections, use `copy-mods.ps1` after building.

If your paths differ, pass MSBuild properties, for example:

```powershell
dotnet build .\ValheimMods\ValheimMods.sln -p:GaleProfile="C:\path\to\your\Gale\profile" -p:ValheimInstall="D:\SteamLibrary\steamapps\common\Valheim"
```

You can set `ValheimPluginsPath` to choose a different `BepInEx\plugins` destination. The copy script accepts `-GaleProfile` and `-ValheimPluginsPath` too. Changing the destination or selecting a Gale profile does not enable copying by itself. See each plugin's README for gameplay details and installation instructions; the copy procedure above applies to all source builds.

## Package for Thunderstore and Gale

Run `powershell -NoProfile -ExecutionPolicy Bypass -File .\package.ps1` from the repository root. This builds the solution in Release configuration and writes one Thunderstore-compatible ZIP per plugin to `dist/`. Each archive contains that plugin's DLL, manifest, README, `CHANGELOG.md`, its own 256 × 256 `icon.png`, and the license. Keep each icon and changelog in its plugin project folder. Packaging copies nothing to the local profile. Pass `-SkipBuild` to package an existing Release build, or `-Plugin UnlimitedStamina` to package one plugin. The package selection does not authorize copying; use `copy-mods.ps1` separately for explicitly selected mods.

To publish locally, install `tcli` with `dotnet tool install tcli --tool-path .tools --version 0.2.4`, create a Thunderstore team service-account token, and save it outside the repository in `%USERPROFILE%\.thunderstore_token`. Run `powershell -NoProfile -ExecutionPolicy Bypass -File .\publish-thunderstore.ps1` to prepare and inspect publisher settings, then add `-Publish` to upload. Use `-Plugin UnlimitedStamina` to upload one plugin. Package versions must match the corresponding `BepInPlugin` versions; `package.ps1` verifies this. The ZIPs, local publisher configuration, and tool installation are ignored by Git.

## Repository layout

```text
ValheimMods/
  ValheimMods.sln                 Visual Studio solution
  Directory.Build.props          Default Gale profile and deployment path
  Directory.Build.targets        Copies only explicitly selected plugin DLLs
  IncreasedCarryWeight/          Player carry weight plugin
  UnlimitedCarryWeight/          Unlimited player carry weight plugin
  ExpandedPlayerInventory/       Player inventory row plugin
  KeepInventoryOnDeath/          Retains inventory after death
  DismantleEquipment/            Recovers materials from crafted items
  ExpandedItemStacks/            Item stack limit plugin
  ExpandedPlaceableChests/       Player-built chest slot plugin
  DevTools/                      Developer console commands
  HelloValheim/                  Minimal logging plugin
  UnlimitedStamina/              Unlimited player stamina plugin
```

Contributions and issue reports are welcome. Keep new plugins in separate projects and use the shared build settings. Copy only mods explicitly selected by the user.

## License

This repository is licensed under the [GNU General Public License v3.0 only](LICENSE) (`GPL-3.0-only`). Copyright (C) 2026 ValheimSuperMods contributors.
