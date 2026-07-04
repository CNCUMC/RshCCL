# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/), and this project adheres to [Semantic Versioning](https://semver.org/).

---

## v3.1.0

### Added

- **RshItemAdapter**: Converts `RshItem` to CUCoreLib `CustomItemInfo` and registers via `ItemRegistry.Register`
- **RshSpawnCallback**: `onSpawn` callback support via CCL `SpawnComponents` mechanism
- **Backwards-compatible API**: Preserves `Plugin.RegisterItem(string, RshItem)` signature, no changes needed for existing mods
- **`krokMpEnabled` field**: Compatibility with NewFirearms and other mods using reflection
- **Conflict avoidance**: Removes self from `Chainloader.PluginInfos` so mods like NewClothing use their native CCL path
- **ConsoleScriptPatch**: `spawn` command autofill includes all CCL-registered items
- **GlobalDarkPatch**: Preserves the "modded" indicator on the main menu
- **RefreshRecipeList guard**: Harmony finalizer catches NRE in CCL recipe list refresh

### Changed

- **Dependency**: Added CUCoreLib ≥ 1.0.1 (hard dependency)
- **Dependency**: KrokoshaCasualtiesMP retained as soft dependency

### Removed

The following features are now handled natively by CUCoreLib and are no longer duplicated:

- `Utils.Create` patches (CCL: `UtilsCreatePatches`)
- `Item.SetupItems` / `Item.GetItem` patches (CCL: `ItemRegistryPatches`)
- `ConPatch` / `InstantiateResourcePatch` (CCL: `KrokMpCompatibilityPatches`)
- Trader item display patches (CCL: `TraderCustomItemPatches`)
- Recipe display patches (CCL: `RecipeRegistryPatches`)
- Wearable lookup patches (CCL: `CustomWearablePatches`)
- Custom network protocol `Network` / `MpValidator` (CCL: `MultiplayerBridge` / `MultiplayerSyncRegistry`)
- Save system `SaveSystemPatch` (CCL: `SaveCoordinator`)
