# 更新日志

本文件记录本项目所有重要变更。

格式基于 [Keep a Changelog](https://keepachangelog.com/)，本项目遵循 [语义化版本控制](https://semver.org/)。

---

## v3.2.2

### 新增

- **Body.UseItem 防护** — Prefix 将可穿戴物品引导至 `WearWearable` 路径；Finalizer 吞掉不可穿戴物品的 NRE（修复
  NewClothing 穿戴道具时崩溃）
- **IngredientNamePatch** — 从原版 RshLib 保留 — 在配方原料列表中显示自定义品质（切割、锤击等）

### 变更

- **krokMpEnabled** — 现在默认为 `false` 并标记 `[Obsolete]`，与原版 RshLib v3.2.0 行为一致。防止 NewFirearms 错误启用 MP
  功能导致崩溃。

### 移除

- **Bark 依赖** — RshLib 不再需要或引用 Bark。所有本地化通过 CCL 的 `LocaleRegistry` 直接处理。
- **Lang/ 目录** — EnLangGenerator、ZhCnLangGenerator、ZhTwLangGenerator — 不再需要。
- **InitializeLocalization()** — 方法已移除。
- **LocaleLog()** — 方法已移除。
- **barkLoaded** — 字段已移除。
- **using System.Reflection** — 导入已移除（不再需要）。
