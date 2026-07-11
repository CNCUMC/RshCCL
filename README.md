![Logo](Logo.png)

[中文指南](README_ZH.md)

> **⚠️ RshCCL is a compatibility bridge only.**
>
> Developers should always prefer using [CUCoreLib](https://github.com/jimmyking9999999/CUCoreLib) directly.
>
> Players should ask mod authors to migrate to CUCoreLib as their API dependency.

# RshCCL

[GitHub](https://github.com/CNCUMC/RshCCL) | [NexusMods](https://www.nexusmods.com/scavprototype/mods/403) | [CUCoreLib](https://github.com/jimmyking9999999/CUCoreLib)

_A compatibility bridge that forwards legacy RshLib API calls to
[CUCoreLib](https://github.com/jimmyking9999999/CUCoreLib) (CCL)._

---

## Overview

**RshCCL** is a drop-in replacement for RshLib *(in most cases)*. It keeps the
`Plugin.RegisterItem(string, RshItem)` API surface so older mods work
unchanged, while internally routing all registrations through CUCoreLib.

- **`RegisterItem`** — Converts `RshItem` → `CustomItemInfo`, then calls `ItemRegistry.Register`
- **`onSpawn` callback** — Injected via CCL `SpawnComponents` + `RshSpawnCallback` MonoBehaviour
- **`krokMpEnabled` / `togetherMpEnabled`** — Provided as backwards-compatible fields
- **Conflict avoidance** — Removes self from `Chainloader.PluginInfos` so mods like NewClothing use their native CCL
  path
- **Console autofill** — Patches `spawn` command to include every CCL-registered item
- **Recipe ingredient display** — Shows custom crafting qualities (cutting, hammering, etc.) in recipe ingredient lists
- **Recipe-list crash guard** — Harmony finalizer catches NRE in `RefreshRecipeList`
- **SaveGame crash guard** — Harmony finalizer catches duplicate key exceptions in CCL SaveCoordinator
- **Body.UseItem guard** — Prefix routes wearable items to `WearWearable` path; finalizer swallows NRE for non-wearable
  items

---

## Installation

1. Install [BepInEx 5.x](https://github.com/BepInEx/BepInEx) for Casualties Unknown.
2. Install [CUCoreLib](https://github.com/jimmyking9999999/CUCoreLib) ≥ 1.0.1 —
   place `CUCoreLib.dll` in `BepInEx/plugins/`.
3. Download the latest `RshCCL.dll` from [Releases](https://github.com/CNCUMC/RshCCL/releases).
4. Place `RshCCL.dll` in `BepInEx/plugins/`.
5. **Remove** the old `RshLib.dll` if it exists.

> ⚠️ RshCCL uses the same BepInEx GUID as old RshLib (`com.rushellxyz.rshlib`).
>
> Only **one** `RshLib.dll` should be present in your plugins folder.

---

## Notes

RshLib itself never provided path or resource-loading APIs. Older mods that
hard-code paths like `"BepInEx/plugins/MyMod/image.png"` must update their
own resource-loading code.

Consider migrating to CCL's
[`AssetLoader`](https://github.com/jimmyking9999999/CUCoreLib) which resolves
paths relative to the calling DLL.

---

## License

[LGPL v3](LICENSE.md)
