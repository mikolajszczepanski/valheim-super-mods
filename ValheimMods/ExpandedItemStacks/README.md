# Expanded Item Stacks

This BepInEx plugin multiplies the original limit of every stackable item by 20: 50 becomes 1,000, 30 becomes 600, and 20 becomes 400. Single-item equipment stays at one per slot. Stack limits are capped at 65,535 because Valheim saves stack counts as unsigned 16-bit values.

Keep this plugin enabled while any inventory or container holds stacks larger than the normal item limit, and back up saves before disabling it.

## Build and install

From the repository root, run `dotnet build .\ValheimMods\ValheimMods.sln`. The shared build target copies `ExpandedItemStacks.dll` to the selected Gale profile's `BepInEx\plugins` folder. Launch Valheim with **Start modded** in Gale.

For multiplayer, players using expanded stacks should install this plugin in their Gale profiles.
