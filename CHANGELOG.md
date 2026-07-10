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
