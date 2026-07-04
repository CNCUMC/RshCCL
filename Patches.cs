using System;
using CUCoreLib.Registries;
using HarmonyLib;
using UnityEngine;

namespace RshLib;

/// <summary>
/// 控制台 spawn 命令自动补全 — 确保通过 RshCCL 注册的自定义物品出现在补全列表中。
/// </summary>
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

/// <summary>
/// 在主菜单 beta build 文字上添加 "modded" 标记，致敬 jimmy_king。
/// </summary>
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

/// <summary>
/// 防护 RefreshRecipeList 中的空引用异常。
/// 当已注册物品（如 icecream 因图片路径问题失败）的图标为 null 时，CCL 可能产生 NRE。
/// </summary>
[HarmonyPatch(typeof(PlayerCamera), "RefreshRecipeList")]
internal class RefreshRecipeListGuardPatch
{
    [HarmonyFinalizer]
    public static Exception Finalizer(Exception __exception)
    {
        if (__exception == null) return null;
        Plugin.Logger.LogWarning($"[RshCCL] Suppressed exception in RefreshRecipeList: {__exception.GetType().Name}: {__exception.Message}");
        return null; // 吞掉异常，配方列表可能不完整但游戏不会崩溃

    }
}