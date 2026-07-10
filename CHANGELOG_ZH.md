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

- **LGPL**: 从GPL v3协议转为LGPL v3。
- **简化日志**：`LogInfo`/`LogWarning`/`LogError` 现在使用简单的 `Regex.Replace` 格式化，而非 locale key。
- **项目结构**：Patch 移至 `Patchers/` 子目录，结构更清晰。
