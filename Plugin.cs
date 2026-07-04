using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace RshLib;

[BepInPlugin(Guid, Name, Version)]
[BepInDependency("net.cucorelib", "1.0.2")]
[BepInDependency("KrokoshaCasualtiesMP", BepInDependency.DependencyFlags.SoftDependency)]
public class Plugin : BaseUnityPlugin
{
    public const string Guid = "com.rushellxyz.rshlib";
    public const string Name = "Rsh CCL";
    public const string Version = "3.1.0";
    
    public static Dictionary<string, RshItem> itemRegistry = new();
    public static bool togetherMpEnabled;
    public static bool krokMpEnabled;
    public static bool anyItemIsRegistred => CUCoreLib.Registries.ItemRegistry.GetRegisteredItemIds().Any();

    internal new static ManualLogSource Logger;
    private readonly Harmony _harmony = new(Guid);

    public void Awake()
    {
        Logger = base.Logger;

        togetherMpEnabled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("CasualtiesMP")
            && 0 == PlayerPrefs.GetInt("CasualtiesMP_FORCE_DISABLE_MP_MOD");
        krokMpEnabled = togetherMpEnabled;

        Logger.LogInfo($"[RshCCL] RshCCL {Version}, CCL bridge active, Together: {togetherMpEnabled}");

        if ("7.0.1" != Application.version)
            Logger.LogWarning($"[RshCCL] ! GAME VERSION MISMATCH, Expected: 7.0.1, Current: {Application.version}, Loading will continue");

        _harmony.PatchAll();

        PreventFalseConflictDetection();

        Logger.LogInfo("RshCCL loaded!");
    }
    
    private static void PreventFalseConflictDetection()
    {
        try
        {
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.Remove(Guid))
                Logger.LogInfo("[RshCCL] Hidden from plugin registry to prevent false conflict detection.");
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
}
