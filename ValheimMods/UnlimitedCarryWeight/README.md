# Unlimited Carry Weight

Carry as much weight as your inventory can hold. This BepInEx plugin removes the player carry weight limit, preventing encumbrance and allowing auto-pickup and tombstone recovery regardless of weight. Inventory slots, stack limits, and individual item weights remain unchanged.

The inventory and controller weight displays show your current weight with an infinity symbol for maximum capacity. Equipment bonuses and world carry weight settings do not impose a limit while this plugin is loaded. There are no settings to configure.

## Install

Install this package with Gale, r2modman, or Thunderstore Mod Manager. The manager installs its Valheim BepInEx pack dependency. Launch Valheim with **Start modded**.

### Manual installation

Install the [Valheim BepInEx pack](https://thunderstore.io/c/valheim/p/denikson/BepInExPack_Valheim/) first. Extract this package's ZIP into a folder, then place the extracted folder containing `UnlimitedCarryWeight.dll` under the active Valheim profile's `BepInEx/plugins` directory. Remove any duplicate or older copies of `UnlimitedCarryWeight.dll` elsewhere in `BepInEx/plugins`, then launch Valheim modded.

For multiplayer, each player who wants unlimited carry weight should install this plugin in their own profile. Available inventory space still limits item pickup and tombstone recovery. This plugin can be used alongside Increased Carry Weight; unlimited capacity takes effect while this plugin is loaded.

## Uninstall

Remove the plugin through your mod manager, or remove its folder and any duplicate copies of `UnlimitedCarryWeight.dll` from the active profile, then restart Valheim. Your normal carry weight limit returns, including bonuses from other mods or equipment. No inventory items are removed by this plugin.

## License

GNU GPL v3.0 only (`GPL-3.0-only`). The package includes the repository's `LICENSE` file. See the [repository license](https://github.com/mikolajszczepanski/valheim-super-mods/blob/main/LICENSE).
