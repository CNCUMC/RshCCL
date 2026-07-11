# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/), and this project adheres
to [Semantic Versioning](https://semver.org/).

---

## v3.2.2

### Added

- **Body.UseItem guard** — Prefix routes wearable items to `WearWearable` path; finalizer swallows NRE for non-wearable
  items (fixes NewClothing crash when equipping items)
- **IngredientNamePatch** — Preserved from original RshLib — displays custom crafting qualities (cutting, hammering,
  etc.) in recipe ingredient lists

### Changed

- **krokMpEnabled** — Now defaults to `false` and marked `[Obsolete]`, matching original RshLib v3.2.0 behavior. This
  prevents NewFirearms from incorrectly enabling MP features and crashing.

### Removed

- **Bark dependency** — RshLib no longer requires or references Bark. All localization is handled directly through CCL's
  `LocaleRegistry`.
- **Lang/ directory** — EnLangGenerator, ZhCnLangGenerator, ZhTwLangGenerator — no longer needed.
- **InitializeLocalization()** — Method removed.
- **LocaleLog()** — Method removed.
- **barkLoaded** — Field removed.
- **using System.Reflection** — Import removed (no longer needed for Bark reflection).
