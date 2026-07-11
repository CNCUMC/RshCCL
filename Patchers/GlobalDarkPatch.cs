using HarmonyLib;
using TMPro;
using UnityEngine;

namespace RshLib.Patchers;

[HarmonyPatch(typeof(GlobalDark))]
internal class GlobalDarkPatch
{
    // 在主菜单 beta build 文字上添加 "modded" 标记，致敬传奇 JimmyKing
    [HarmonyPatch("Awake")]
    private static void Postfix(GlobalDark __instance)
    {
        var betaBuildObj = GameObject.Find("GlobalDark(Clone)/betabuild");
        if (betaBuildObj == null) return;

        var betaBuildText = betaBuildObj.GetComponent<TMP_Text>();
        if (betaBuildText == null) return;

        if (!betaBuildText.text.Contains(" modded"))
            betaBuildText.text = betaBuildText.text.Replace("This is a ", "This is a modded ");

        var textColor = betaBuildText.color;
        textColor.a = 0.0227f;
        betaBuildText.color = textColor;
    }
}