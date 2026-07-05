using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using Bark.BetterCCL;
using BepInEx;
using BepInEx.Logging;
using CUCoreLib.Helpers;
using CUCoreLib.Registries;
using HarmonyLib;
using RshLib.Lang;
using UnityEngine;

namespace RshLib;

[BepInPlugin(Guid, Name, Version)]
[BepInDependency("net.cucorelib", "1.0.2")]
[BepInDependency("KrokoshaCasualtiesMP", BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency("org.cucnmc.bark", BepInDependency.DependencyFlags.SoftDependency)]

[SuppressMessage("ReSharper", "NotAccessedField.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class Plugin : BaseUnityPlugin
{
    public const string Guid = "com.rushellxyz.rshlib";
    public const string Name = "RshCCL";
    public const string Version = "3.1.1";

    // ReSharper disable once FieldCanBeMadeReadOnly.Global
    // ReSharper disable once CollectionNeverQueried.Global
    public static Dictionary<string, RshItem> itemRegistry = new();
    public static bool togetherMpEnabled;
    public static bool krokMpEnabled;
    public static bool anyItemIsRegistred => ItemRegistry.GetRegisteredItemIds().Any();
    internal static bool barkLoaded;

    internal new static ManualLogSource Logger;
    private readonly Harmony _harmony = new(Guid);

    public void Awake()
    {
        Logger = base.Logger;

        togetherMpEnabled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("CasualtiesMP")
                            && 0 == PlayerPrefs.GetInt("CasualtiesMP_FORCE_DISABLE_MP_MOD");
        krokMpEnabled = togetherMpEnabled;

        LogInfo("version", "RshCCL {0}, CUCoreLib bridge active, Together: {1}", Version, togetherMpEnabled);

        if ("7.0.1" != Application.version)
            LogWarning("game_version_mismatch",
                "GAME VERSION MISMATCH, Expected: 7.0.1, Current: {0}, Loading will continue", Application.version);

        _harmony.PatchAll();

        PreventFalseConflictDetection();

        InitializeLocalization();

        LogInfo("loaded", "RshCCL loaded!");
    }

    private static void InitializeLocalization()
    {
        if (!BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("org.cucnmc.bark"))
        {
            return;
        }

        barkLoaded = true;
        new EnLangGenerator().Initialize(Logger);
        new ZhCnLangGenerator().Initialize(Logger);
        new ZhTwLangGenerator().Initialize(Logger);
        BetterLocale.Flush();
    }

    private static void PreventFalseConflictDetection()
    {
        try
        {
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.Remove(Guid))
                LogInfo("hide", "Hidden from plugin registry to prevent false conflict detection.");
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

    internal static void LogError(string key, string text, params object[] args)
    {
        var message = barkLoaded
            ? LocaleLog(key, args)
            : Replace(text, args);
        Logger.LogError(message);
        CUCoreUtils.ConsoleLog(ConsoleScript.instance, "[ERROR][RshCCL]" + message);
    }

    internal static void LogInfo(string key, string text, params object[] args)
    {
        var message = barkLoaded
            ? LocaleLog(key, args)
            : Replace(text, args);
        Logger.LogInfo(message);
        CUCoreUtils.ConsoleLog(ConsoleScript.instance, "[RshCCL]" + message);
    }

    internal static void LogWarning(string key, string text, params object[] args)
    {
        var message = barkLoaded
            ? LocaleLog(key, args)
            : Replace(text, args);
        Logger.LogWarning(message);
        CUCoreUtils.ConsoleLog(ConsoleScript.instance, "[WARNING][RshCCL]" + message);
    }

    private static string Replace(string key, params object[] args)
    {
        return args == null || args.Length == 0
            ? key
            : Regex.Replace(key, @"\{(\d+)\}",
                match => args[int.Parse(match.Groups[1].Value)].ToString());
    }

    internal static string LocaleLog(string key, params object[] args)
    {
        return BetterLocale.GetLog(key, args);
    }
}