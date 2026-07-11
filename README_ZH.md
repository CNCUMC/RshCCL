![Logo](Logo.png)

[English Guide](README.md)

> **⚠️ RshCCL 仅作为兼容层存在。**
>
> 开发者应始终优先直接使用 [CUCoreLib](https://github.com/jimmyking9999999/CUCoreLib) 作为前置。
>
> 玩家应提醒模组作者将 API 迁移至 CUCoreLib。

# RshCCL

[GitHub](https://github.com/CNCUMC/RshCCL) | [NexusMods](https://www.nexusmods.com/scavprototype/mods/403) | [CUCoreLib](https://github.com/jimmyking9999999/CUCoreLib)

_一个兼容转接层，将旧 RshLib 的 API 调用转发到
[CUCoreLib](https://github.com/jimmyking9999999/CUCoreLib) （CCL）。_

---

## 概述

**RshCCL** 是 RshLib 的无感替代 *（大部分情况下）*。它保留了 `Plugin.RegisterItem(string, RshItem)` API
签名，老模组无需修改即可运行，内部将所有注册转发至 CUCoreLib。

- **`RegisterItem`** — 将 `RshItem` 转换为 `CustomItemInfo`，调用 `ItemRegistry.Register`
- **`onSpawn` 回调** — 通过 CCL `SpawnComponents` + `RshSpawnCallback` MonoBehaviour 注入
- **`krokMpEnabled` / `togetherMpEnabled`** — 提供向后兼容字段
- **冲突避免** — 从 `Chainloader.PluginInfos` 移除自身，让 NewClothing 等模组走 CCL 原生路径
- **控制台自动补全** — Patch `spawn` 命令，包含所有 CCL 注册的物品
- **配方原料显示** — 在配方原料列表中显示自定义品质（切割、锤击等）
- **配方列表防护** — Harmony finalizer 捕获 `RefreshRecipeList` 中的 NRE
- **存档防护** — Harmony finalizer 捕获 CCL SaveCoordinator 重复键异常
- **Body.UseItem 防护** — Prefix 将可穿戴物品引导至 `WearWearable` 路径；Finalizer 吞掉不可穿戴物品的 NRE

---

## 安装

1. 为 Casualties Unknown 安装 [BepInEx 5.x](https://github.com/BepInEx/BepInEx)。
2. 安装 [CUCoreLib](https://github.com/jimmyking9999999/CUCoreLib) ≥ 1.0.1 —
   将 `CUCoreLib.dll` 放入 `BepInEx/plugins/`。
3. 从 [Releases](https://github.com/CNCUMC/RshCCL/releases) 下载最新版 `RshCCL.dll`。
4. 将 `RshCCL.dll` 放入 `BepInEx/plugins/`。
5. **删除** 旧版 `RshLib.dll`（如果存在）。

> ⚠️ RshCCL 使用与旧 RshLib 相同的 BepInEx GUID (`com.rushellxyz.rshlib`)。
>
> `plugins` 目录下**只能有一个** `RshLib.dll`。

---

## 注意事项

RshLib 从未提供路径或资源加载 API。老模组中硬编码的路径（如 `"BepInEx/plugins/MyMod/image.png"`）需要模组自身更新资源加载代码。

建议迁移到 CCL 的
[`AssetLoader`](https://github.com/jimmyking9999999/CUCoreLib)，它支持相对于 DLL 的动态路径。

---

## License

[LGPL v3](LICENSE.md)
