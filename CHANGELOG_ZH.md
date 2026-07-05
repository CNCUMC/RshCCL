# 更新日志

本文件记录本项目所有重要变更。

格式基于 [Keep a Changelog](https://keepachangelog.com/)，本项目遵循 [语义化版本控制](https://semver.org/)。

---

## v3.1.1

### 新增

- **SaveGame 防护**: Harmony finalizer 捕获 CCL SaveCoordinator 重复组件键的 `ArgumentException`（如 NewFirearms.RshGun）
- **Bark 本地化**（可选前置）: 自动检测缺失翻译，通过 `Lang/EnLangGenerator`、`ZhCnLangGenerator`、`ZhTwLangGenerator` 注册 EN/zh-CN/zh-TW 回退值
- **基于反射的 Bark 集成**: 不硬引用 Bark.dll — Bark 不存在时优雅降级
- **日志本地化系统**: `LogInfo`/`LogWarning`/`LogError` 使用 locale key，回退到英文 `Regex.Replace`

### 变更

- **依赖**: CUCoreLib ≥ 1.0.1 → 1.0.2
- **依赖**: 新增 Bark `org.cucnmc.bark` 作为可选依赖
- **模组名**: `"Rsh CCL"` → `"RshCCL"`（去掉空格）
- **冲突避免**: 从"永久移除自身"改为"临时隐藏 → Chainloader.Start Postfix 恢复"，避免依赖 RshLib 的后加载模组找不到前置
- **StartGame.ps1**: `$ModNamespace = "RshLib"`（DLL 名），`$ModName = "RshCCL"`（文件夹/显示名）

---

