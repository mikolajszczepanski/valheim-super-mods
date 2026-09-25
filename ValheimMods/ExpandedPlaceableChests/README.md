# Expanded Placeable Chests

This BepInEx plugin triples the number of slots in player-built chests. It adds rows while keeping each chest's original width, so existing item positions stay the same. It applies to wooden, reinforced, black metal, and other placeable chest variants, including existing chests when they load. The chest inventory can scroll to show the added rows. Carts, ships, graves, and loot containers keep their normal sizes.

Keep this plugin enabled while chest items occupy the added rows, and back up saves before disabling it.

## Build and install

From the repository root, run `dotnet build .\ValheimMods\ValheimMods.sln`. The shared build target copies `ExpandedPlaceableChests.dll` to the selected Gale profile's `BepInEx\plugins` folder. Launch Valheim with **Start modded** in Gale.

For multiplayer, players accessing expanded chests should install this plugin in their Gale profiles.
