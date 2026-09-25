# Keep Inventory On Death

This BepInEx plugin keeps every item in the player's inventory when they die, including equipped items. It prevents the game from creating a tombstone and moving items into it. The normal death and respawn sequence still runs, including the world's skill loss rules.

## Build and install

From the repository root, run `dotnet build .\ValheimMods\ValheimMods.sln`. The shared build target copies `KeepInventoryOnDeath.dll` to the selected Gale profile's `BepInEx\plugins` folder. Launch Valheim with **Start modded** in Gale.

For multiplayer, each player who wants to keep their inventory should install this plugin in their own Gale profile. It does not restore items lost before the plugin was installed.
