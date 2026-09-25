# Extended Player Capacity

This BepInEx plugin gives each player 10 times the base carry weight (3,000 instead of 300) and at least 64 inventory slots (8 columns by 8 rows). Every item that normally stacks gets 20 times its original stack limit: 50 becomes 1,000, 30 becomes 600, and 20 becomes 400. Single-item equipment stays at one per slot. Equipment still occupies inventory slots, as in Valheim's standard inventory.

The extra rows are applied when a player spawns and saved with the character. Removing the plugin does not automatically shrink a character's inventory back to 32 slots. An existing inventory larger than 64 slots is left alone to avoid dropping items.

Keep the plugin enabled while any inventory or container holds stacks larger than the item's normal limit, and back up your saves before removing it. Valheim's normal stack limits would apply again without the plugin. Stack limits are capped at 65,535 because Valheim saves stack counts as unsigned 16-bit values.

## Build and install

From the `ValheimMods` solution folder, run `dotnet build .\ValheimMods.sln`. The shared build target copies `ExtendedPlayerCapacity.dll` to the Gale `Default` profile's `BepInEx\plugins` folder. Launch Valheim using Gale's **Launch modded** button.

For multiplayer, each player who wants the increased capacity should install the plugin in their own Gale profile.

## Optional: enable the developer console

The console is not required for this plugin. To use Valheim commands while launching through Gale, select the **Default** profile, then click the **Settings** gear in the far-left sidebar. It is circled below, directly under the cube icon:

![Gale Settings gear circled in the left sidebar](assets/gale-settings.png)

On the Settings page, check **Valheim settings → Launch → Launch mode**:

- If it is **Direct**, scroll to **Profile settings → Launch → Custom launch arguments** and enter `-console`.
- If it is **Platform (Steam)**, add `-console` in **Steam → Valheim → Properties → General → Launch Options** instead.

Then launch the game modded through Gale. In your own world, press **F5** to open the console and enter `devcommands`. To spawn one each of 60 different materials with a single command, enter `spawn60`. If Valheim asks you to confirm cheats, enter `confirmcheats` and then repeat `spawn60`. The items appear near your character.

`spawn60` uses Valheim's built-in `spawn` command for each item. Cheat commands and spawned items can affect achievements in Valheim 1.0.
