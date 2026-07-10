# 更新日志

本文件记录本项目所有重要变更。

格式基于 [Keep a Changelog](https://keepachangelog.com/)，本项目遵循 [语义化版本控制](https://semver.org/)。

---

## v3.1.2

### 移除

- **Bark 依赖**：RshCCL 不再需要或引用 Bark。所有本地化通过 CCL 的 `LocaleRegistry` 直接处理。
- **Lang/ 目录**：EnLangGenerator、ZhCnLangGenerator、ZhTwLangGenerator — 不再需要。
- **InitializeLocalization()**：方法已移除。
- **LocaleLog()**：方法已移除。
- **barkLoaded**：字段已移除。
- **using System.Reflection**：导入已移除（不再需要）。

### 变更

- **简化日志**：`LogInfo`/`LogWarning`/`LogError` 现在使用简单的 `Regex.Replace` 格式化，而非 locale key。
- **项目结构**：Patch 移至 `Patchers/` 子目录，结构更清晰。

---

## v3.1.1

### 新增

- **SaveGame 防护**: Harmony finalizer 捕获 CCL SaveCoordinator 重复组件键的 `ArgumentException`（如 NewFirearms.RshGun）。
- **customItems 字段**: `Dictionary<string, Sprite>`，兼容 Re-Growth Serums 等旧模组。
- **IngredientNamePatch**: 从原版 RshLib 保留 — 在配方原料列表中显示自定义品质（切割、锤击等）。

### 变更

- **依赖**: CUCoreLib ≥ 1.0.2。
- **模组名**: `"Rsh CCL"` → `"RshCCL"`（去掉空格）。
- **冲突避免**: 从"永久移除自身"改为"临时隐藏 → Chainloader.Start Postfix 恢复"，避免依赖 RshLib 的后加载模组找不到前置。
- **krokMpEnabled**: 现在默认为 `false` 并标记 `[Obsolete]`，与原版 RshLib v3.2.0 行为一致。防止 NewFirearms 错误启用 MP 功能。
- **StartGame.ps1**: `$ModNamespace = "RshLib"`（DLL 名），`$ModName = "RshCCL"`（文件夹/显示名）。

### 移除

- MP 专用 Patch（`MpValidator`、`MpInstantiateResourcePatch`、`FixTradersForClientsWithoutRshlib`）— 全部由 CCL 原生处理。
