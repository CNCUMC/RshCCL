using System;
using CUCoreLib.Registries;
using HarmonyLib;
using UnityEngine;

namespace RshLib;

[HarmonyPatch(typeof(ConsoleScript))]
internal class ConsoleScriptPatch
{
    [HarmonyPatch("RegisterSpawnEntities")]
    private static void Postfix(ConsoleScript __instance)
    {
        var command = ConsoleScript.SearchExact("spawn");
        if (command == null) return;

        foreach (var itemId in ItemRegistry.GetRegisteredItemIds())
        {
            if (!command.argAutofill[0].Contains(itemId))
                command.argAutofill[0].Add(itemId);
        }
    }
}

// 在主菜单 beta build 文字上添加 "modded" 标记，致敬传奇 JimmyKing
[HarmonyPatch(typeof(GlobalDark))]
internal class GlobalDarkPatch
{
    [HarmonyPatch("Awake")]
    private static void Postfix(GlobalDark __instance)
    {
        var betaBuildObj = GameObject.Find("GlobalDark(Clone)/betabuild");
        if (betaBuildObj == null) return;

        var betaBuildText = betaBuildObj.GetComponent<TMPro.TMP_Text>();
        if (betaBuildText == null) return;

        if (!betaBuildText.text.Contains(" modded"))
            betaBuildText.text = betaBuildText.text.Replace("This is a ", "This is a modded ");

        var textColor = betaBuildText.color;
        textColor.a = 0.0227f;
        betaBuildText.color = textColor;
    }
}

[HarmonyPatch(typeof(PlayerCamera))]
internal class RefreshRecipeListGuardPatch
{
    [HarmonyPatch("RefreshRecipeList")]
    [HarmonyFinalizer]
    public static Exception Finalizer(Exception __exception)
    {
        if (__exception == null) return null;
        Plugin.LogWarning("refreshrecipelist_suppressed_exception", "Suppressed exception in RefreshRecipeList: {0}: {1}", __exception.GetType().Name, __exception.Message);
        return null;
    }
}

[HarmonyPatch(typeof(SaveSystem))]
internal class SaveGameGuardPatch
{
    [HarmonyPatch("SaveGame")]
    [HarmonyFinalizer]
    public static Exception Finalizer(Exception __exception)
    {
        switch (__exception)
        {
            case null:
                return null;
            case ArgumentException argEx
                when argEx.Message.Contains("same key has already been added"):
                Plugin.LogWarning("savegame_duplicate_key_suppressed",
                    "Suppressed duplicate key in SaveGame: {0}", argEx.Message);
                return null; // 吞掉异常，存档继续
            default:
                return __exception; // 其他异常继续抛出
            }
        }
}

[HarmonyPatch(typeof(BepInEx.Bootstrap.Chainloader))]
internal class ChainloaderStartPostfix
{
    [HarmonyPatch("Start")]
    [HarmonyPostfix]
    private static void Postfix()
    {
        Plugin.RestoreToPluginInfos();
    }
}