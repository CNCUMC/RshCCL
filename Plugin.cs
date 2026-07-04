using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace RshLib;

[BepInPlugin(Guid, Name, Version)]
[BepInDependency("net.cucorelib", "1.0.1")]
[BepInDependency("KrokoshaCasualtiesMP", BepInDependency.DependencyFlags.SoftDependency)]
public class Plugin : BaseUnityPlugin
{
    public const string Guid = "com.rushellxyz.rshlib";
    public const string Name = "Rsh CCL";
    public const string Version = "3.1.0";
    
    /// <summary>向后兼容的旧注册表。新代码应使用 CUCoreLib.ItemRegistry。</summary>
    public static Dictionary<string, RshItem> itemRegistry = new();

    /// <summary>CasualtiesMP 是否已启用。</summary>
    public static bool togetherMpEnabled;

    /// <summary>旧版字段别名，兼容 NewFirearms 等老模组。注意：必须是字段而非属性，反射才能找到。</summary>
    public static bool krokMpEnabled;

    /// <summary>是否有任何物品已注册（通过 RshCCL 或 CCL）。</summary>
    public static bool anyItemIsRegistred => CUCoreLib.Registries.ItemRegistry.GetRegisteredItemIds().Any();

    internal new static ManualLogSource Logger;
    private readonly Harmony _harmony = new(Guid);

    public void Awake()
    {
        Logger = base.Logger;

        togetherMpEnabled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("CasualtiesMP")
            && 0 == PlayerPrefs.GetInt("CasualtiesMP_FORCE_DISABLE_MP_MOD");
        krokMpEnabled = togetherMpEnabled; // 同步旧字段

        Logger.LogInfo($"[RshCCL] RshCCL {Version}, CCL bridge active, Together: {togetherMpEnabled}");

        if ("7.0.1" != Application.version)
            Logger.LogWarning($"[RshCCL] ! GAME VERSION MISMATCH, Expected: 7.0.1, Current: {Application.version}, Loading will continue");

        _harmony.PatchAll();

        // 避免 NewClothing 等模组检测到 RshLib + CCL "冲突"（RshCCL 是桥接层，不会冲突）
        PreventFalseConflictDetection();

        Logger.LogInfo("RshCCL loaded!");
    }

    /// <summary>
    /// 从 BepInEx PluginInfos 中移除自身条目，避免 NewClothing 等模组
    /// 因检测到 "RshLib + CUCoreLib 同时存在" 而拒绝加载。
    /// 此时所有依赖检查已完成，移除不会影响加载顺序。
    /// </summary>
    private static void PreventFalseConflictDetection()
    {
        try
        {
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.Remove(Guid))
                Logger.LogInfo("[RshCCL] Hidden from plugin registry to prevent false conflict detection.");
        }
        catch
        {
            // 忽略，不影响功能
        }
    }

    // ===== 向后兼容的公共 API =====

    /// <summary>
    /// 注册自定义物品。与旧 RshLib API 完全兼容。
    /// 内部将 RshItem 转换为 CUCoreLib 的 CustomItemInfo 并调用 ItemRegistry.Register。
    /// </summary>
    public static void RegisterItem(string itemId, RshItem rshItem)
    {
        itemRegistry[itemId] = rshItem;
        RshItemAdapter.RegisterItem(itemId, rshItem);
    }

    // ===== 辅助方法（从旧 RshLib 保留） =====

    /// <summary>安装 Harmony Prefix Patch。</summary>
    internal static void PatchPrefix(Harmony harmony, string targetClass, string targetMethod, string prefixClass)
    {
        var target = AccessTools.Method(AccessTools.TypeByName(targetClass), targetMethod);
        var prefix = AccessTools.Method(Type.GetType(prefixClass), "Prefix");
        harmony.Patch(target, prefix: new HarmonyMethod(prefix));
    }

    /// <summary>安装 Harmony Postfix Patch。</summary>
    internal static void PatchPostfix(Harmony harmony, string targetClass, string targetMethod, string postfixClass)
    {
        var target = AccessTools.Method(AccessTools.TypeByName(targetClass), targetMethod);
        var postfix = AccessTools.Method(Type.GetType(postfixClass), "Postfix");
        harmony.Patch(target, postfix: new HarmonyMethod(postfix));
    }

    /// <summary>
    /// 获取多人游戏存档路径。
    /// 如果 CasualtiesMP 不可用，回退到原生 persistentDataPath。
    /// </summary>
    internal static string GetMPSavePath()
    {
        try
        {
            var type = Type.GetType("Together.SavesystemPatch, KrokoshaCasualtiesMP");
            if (type != null)
            {
                var method = type.GetMethod("ReplacementFor_Application_persistentDataPath");
                if (method != null)
                    return (string)method.Invoke(null, null);
            }
        }
        catch
        {
            // ignored
        }

        return Application.persistentDataPath;
    }
}
