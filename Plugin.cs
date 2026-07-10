using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BepInEx;
using BepInEx.Logging;
using CUCoreLib.Helpers;
using CUCoreLib.Registries;
using HarmonyLib;
using UnityEngine;

namespace RshLib;

[BepInPlugin(Guid, Name, Version)]
[BepInDependency("net.cucorelib", "1.0.2")]
[BepInDependency("KrokoshaCasualtiesMP", BepInDependency.DependencyFlags.SoftDependency)]
public class Plugin : BaseUnityPlugin
{
    public const string Guid = "com.rushellxyz.rshlib";
    public const string Name = "RshCCL";
    public const string Version = "3.2.1";

    // ReSharper disable once FieldCanBeMadeReadOnly.Global
    // ReSharper disable once CollectionNeverQueried.Global
    public static Dictionary<string, RshItem> itemRegistry = new();
    public static Dictionary<string, Sprite> customItems = new();
    public static bool togetherMpEnabled;

    [Obsolete("Use togetherMpEnabled instead.")]
    public static bool krokMpEnabled;

    public static bool anyItemIsRegistred => ItemRegistry.GetRegisteredItemIds().Any();

    internal new static ManualLogSource Logger;
    private readonly Harmony _harmony = new(Guid);
    private static PluginInfo _savedPluginInfo;

    public void Awake()
    {
        Logger = base.Logger;

        togetherMpEnabled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("CasualtiesMP")
                            && 0 == PlayerPrefs.GetInt("CasualtiesMP_FORCE_DISABLE_MP_MOD");

        LogInfo("RshCCL {0}, CUCoreLib bridge active, Together: {1}", Version, togetherMpEnabled);

        if ("7.0.1" != Application.version)
            LogWarning("GAME VERSION MISMATCH, Expected: 7.0.1, Current: {0}, Loading will continue",
                Application.version);

        _harmony.PatchAll();

        PreventFalseConflictDetection();

        LogInfo("RshCCL loaded!");
    }

    private static void PreventFalseConflictDetection()
    {
        try
        {
            if (!BepInEx.Bootstrap.Chainloader.PluginInfos.TryGetValue(Guid, out var info)) return;
            _savedPluginInfo = info;
            BepInEx.Bootstrap.Chainloader.PluginInfos.Remove(Guid);
            LogInfo("Hidden from plugin registry to prevent false conflict detection.");
        }
        catch
        {
            // ignored
        }
    }

    internal static void RestoreToPluginInfos()
    {
        if (_savedPluginInfo == null) return;
        if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(Guid)) return;

        try
        {
            BepInEx.Bootstrap.Chainloader.PluginInfos[Guid] = _savedPluginInfo;
            LogInfo("Restored to plugin registry after all mods loaded.");
        }
        catch
        {
            // ignored
        }
    }

    public static void RegisterItem(string itemId, RshItem rshItem)
    {
        itemRegistry[itemId] = rshItem;
        RshItemAdapter.RegisterItem(itemId, rshItem);
    }

    internal static void PatchPrefix(Harmony harmony, string targetClass, string targetMethod, string prefixClass)
    {
        var target = AccessTools.Method(AccessTools.TypeByName(targetClass), targetMethod);
        var prefix = AccessTools.Method(Type.GetType(prefixClass), "Prefix");
        harmony.Patch(target, prefix: new HarmonyMethod(prefix));
    }

    internal static void PatchPostfix(Harmony harmony, string targetClass, string targetMethod, string postfixClass)
    {
        var target = AccessTools.Method(AccessTools.TypeByName(targetClass), targetMethod);
        var postfix = AccessTools.Method(Type.GetType(postfixClass), "Postfix");
        harmony.Patch(target, postfix: new HarmonyMethod(postfix));
    }

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

    internal static void LogError(string text, params object[] args)
    {
        var message = Format(text, args);
        Logger.LogError(message);
        CUCoreUtils.ConsoleLog(ConsoleScript.instance, "[ERROR][RshCCL]" + message);
    }

    internal static void LogInfo(string text, params object[] args)
    {
        var message = Format(text, args);
        Logger.LogInfo(message);
        CUCoreUtils.ConsoleLog(ConsoleScript.instance, "[RshCCL]" + message);
    }

    internal static void LogWarning(string text, params object[] args)
    {
        var message = Format(text, args);
        Logger.LogWarning(message);
        CUCoreUtils.ConsoleLog(ConsoleScript.instance, "[WARNING][RshCCL]" + message);
    }

    private static string Format(string text, params object[] args)
    {
        return args == null || args.Length == 0
            ? text
            : Regex.Replace(text, @"\{(\d+)\}",
                match => args[int.Parse(match.Groups[1].Value)].ToString());
    }
}