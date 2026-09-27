# Dismantle Equipment

With the inventory open, hold **Shift** and **right-click** an item in the player's inventory. The item is dismantled immediately, its materials drop at the player's feet, and a short message confirms what was dismantled. Ordinary right-click still equips or uses items as usual.

The item must have an enabled crafting recipe with known ingredients. For recipes that produce several items (such as ammunition), each action dismantles one complete crafting batch from the stack. A partial batch cannot be dismantled. Quality upgrades return the base ingredients plus the ingredients for each completed upgrade level, using the requirements for that level's crafting station. Recipes with alternative ingredients cannot be dismantled because Valheim does not record which alternative was originally used.

This works on equipped items; they are unequipped before removal. The recovered materials are split into normal item stacks.

## Install

Install this package with Gale, r2modman, or Thunderstore Mod Manager. The manager installs its Valheim BepInEx pack dependency. Launch Valheim with **Start modded**.

### Manual installation

Install the [Valheim BepInEx pack](https://thunderstore.io/c/valheim/p/denikson/BepInExPack_Valheim/) first. Download this package's ZIP, extract it, and place the extracted folder under the active Valheim profile's `BepInEx/plugins` directory. Keep `DismantleEquipment.dll` inside that folder; `manifest.json`, `README.md`, `icon.png`, and `LICENSE` can stay there too. Remove any older copy of `DismantleEquipment.dll` elsewhere in `BepInEx/plugins`, then launch Valheim modded.

## Build from source

Run `dotnet build .\ValheimMods\ValheimMods.sln` from the repository root. The shared build target copies `DismantleEquipment.dll` to the selected Gale profile's `BepInEx\plugins` folder. Start Valheim with **Start modded** in Gale.

## License

GNU GPL v3.0 only (`GPL-3.0-only`). See the `LICENSE` file included in the package.
