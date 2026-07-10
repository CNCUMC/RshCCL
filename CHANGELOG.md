# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/), and this project adheres to [Semantic Versioning](https://semver.org/).

---

## v3.1.2

### Removed

- **Bark dependency**: RshCCL no longer requires or references Bark. All localization is handled directly through CCL's `LocaleRegistry`.
- **Lang/ directory**: EnLangGenerator, ZhCnLangGenerator, ZhTwLangGenerator — no longer needed.
- **InitializeLocalization()**: Method removed.
- **LocaleLog()**: Method removed.
- **barkLoaded**: Field removed.
- **using System.Reflection**: Import removed (no longer needed for Bark reflection).

### Changed

- **Simplified logging**: `LogInfo`/`LogWarning`/`LogError` now use simple `Regex.Replace` formatting instead of locale keys.
- **Project structure**: Patches moved to `Patchers/` subdirectory for better organization.

---

## v3.1.1

### Added

- **SaveGame guard**: Harmony finalizer catches `ArgumentException` from CCL SaveCoordinator duplicate component keys (e.g. NewFirearms.RshGun).
- **customItems field**: `Dictionary<string, Sprite>` for backwards compatibility with mods like Re-Growth Serums.
- **IngredientNamePatch**: Preserved from original RshLib — displays custom crafting qualities (cutting, hammering, etc.) in recipe ingredient lists.

### Changed

- **Dependency**: CUCoreLib ≥ 1.0.2.
- **Mod name**: `"Rsh CCL"` → `"RshCCL"` (no space).
- **Conflict avoidance**: Changed from "permanently remove self" to "temporarily hide → Chainloader.Start Postfix restore", so mods that depend on RshLib can still find it after loading.
- **krokMpEnabled**: Now defaults to `false` and marked `[Obsolete]`, matching original RshLib v3.2.0 behavior. This prevents NewFirearms from incorrectly enabling MP features.
- **StartGame.ps1**: `$ModNamespace = "RshLib"` for DLL name, `$ModName = "RshCCL"` for folder/display.

### Removed

- MP-only patches (`MpValidator`, `MpInstantiateResourcePatch`, `FixTradersForClientsWithoutRshlib`) — all handled natively by CCL.
