# 更新日志

本文件记录本项目所有重要变更。

格式基于 [Keep a Changelog](https://keepachangelog.com/)，本项目遵循 [语义化版本控制](https://semver.org/)。

---

## v3.1.0

### 新增

- **RshItemAdapter**: 自动将 `RshItem` 转换为 CUCoreLib 的 `CustomItemInfo`，并通过 `ItemRegistry.Register` 注册
- **RshSpawnCallback**: 通过 CCL 的 `SpawnComponents` 机制实现 `onSpawn` 回调支持
- **向后兼容 API**: 保留 `Plugin.RegisterItem(string, RshItem)` 签名，老模组无需修改
- **`krokMpEnabled` 字段**: 兼容 NewFirearms 等旧模组的反射访问
- **冲突避免**: 从 `Chainloader.PluginInfos` 移除自身，让 NewClothing 等模组走 CCL 原生路径
- **ConsoleScriptPatch**: `spawn` 命令自动补全支持所有 CCL 注册的物品
- **GlobalDarkPatch**: 保留 "modded" 标志显示
- **RefreshRecipeList 防护**: Harmony finalizer 捕获 CCL 配方刷新中的 NRE

### 变更

- **依赖**: 新增 CUCoreLib ≥ 1.0.1（硬依赖）
- **依赖**: 保留 KrokoshaCasualtiesMP（软依赖）

### 移除

以下功能已被 CUCoreLib 原生覆盖，不再需要 RshCCL 重复实现：

- `Utils.Create` Patch（CCL: `UtilsCreatePatches`）
- `Item.SetupItems` / `Item.GetItem` Patch（CCL: `ItemRegistryPatches`）
- `ConPatch` / `InstantiateResourcePatch`（CCL: `KrokMpCompatibilityPatches`）
- 商人物品显示 Patch（CCL: `TraderCustomItemPatches`）
- 配方显示 Patch（CCL: `RecipeRegistryPatches`）
- 穿戴物品查找 Patch（CCL: `CustomWearablePatches`）
- 自定义网络协议 `Network` / `MpValidator`（CCL: `MultiplayerBridge` / `MultiplayerSyncRegistry`）
- 存档系统 `SaveSystemPatch`（CCL: `SaveCoordinator`）
