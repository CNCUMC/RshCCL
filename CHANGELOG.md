# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/), and this project adheres to [Semantic Versioning](https://semver.org/).

---

## v3.1.1

### Added

- **SaveGame guard**: Harmony finalizer catches `ArgumentException` from CCL SaveCoordinator duplicate component keys (e.g. NewFirearms.RshGun)
- **Bark localization** (soft dependency): auto-detects missing translations, registers EN/zh-CN/zh-TW fallbacks via `Lang/EnLangGenerator`, `ZhCnLangGenerator`, `ZhTwLangGenerator`
- **Reflection-based Bark integration**: No hard reference to Bark.dll — gracefully degrades when Bark is absent
- **Log localization system**: `LogInfo`/`LogWarning`/`LogError` with locale keys, falling back to English `Regex.Replace`

### Changed

- **Dependency**: CUCoreLib ≥ 1.0.1 → 1.0.2
- **Dependency**: Bark `org.cucnmc.bark` added as soft dependency
- **Mod name**: `"Rsh CCL"` → `"RshCCL"` (no space)
- **Conflict avoidance**: Changed from "permanently remove self" to "temporarily hide → Chainloader.Start Postfix restore", so mods that depend on RshLib can still find it after loading
- **StartGame.ps1**: `$ModNamespace = "RshLib"` for DLL name, `$ModName = "RshCCL"` for folder/display

---

