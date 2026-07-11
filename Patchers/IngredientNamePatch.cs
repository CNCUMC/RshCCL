using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace RshLib.Patchers;

[HarmonyPatch(typeof(PlayerCamera))]
internal class IngredientNamePatch
{
    [HarmonyPatch("IngredientTextForRecipe")]
    private static bool Prefix(PlayerCamera __instance, ref string __result, Recipe recipe, List<Item> its = null)
    {
        __result = "";
        var items = its ?? recipe.GetItemsForRecipeThorough();
        for (var i = 0; i < recipe.items.Count; i++)
        {
            var recit = recipe.items[i];
            __result += "<color=#FFFFFF>";
            __result += items[i] ? "<sprite index=23>" : "<sprite index=24>";
            __result = recit.specific
                ? !recit.isLiquid
                    ? __result + Item.GetItem(recit.specificId).fullName + "\n"
                    : __result + Locale.GetOther(recit.specificId) + "\n"
                : !recit.isLiquid
                    ? __result + Locale.GetOther(recit.quality.id == "hammering" || recit.quality.id == "cutting"
                        ? "craftanytool"
                        : "craftanyitem") + "\n"
                    : __result + Locale.GetOther("craftanyliquid") + "\n";
            __result += "<color=#666666>";
            if (recit.isLiquid)
            {
                if (!recit.specific)
                    __result = __result + Locale.GetOther("craftliquidquality")
                                   .Replace("<1>", recit.quality.amount.ToString("0.#"))
                                   .Replace("<2>", recit.quality.LocaleName) +
                               "\n";
                else if (recit.minimumCondition > 0f)
                    __result = __result +
                               Locale.GetOther("craftml").Replace("<>", recit.minimumCondition.ToString("0.#")) + "\n";

                continue;
            }

            if (!recit.specific)
            {
                __result += Locale.GetOther("craftitemquality").Replace("<1>", recit.quality.amount.ToString("0.#"))
                    .Replace("<2>", recit.quality.LocaleName);
                var keyValuePair = Recipes.QualityExamples.FirstOrDefault(x =>
                    x.Key.id == recit.quality.id && Mathf.Approximately(x.Key.amount, recit.quality.amount));
                if (keyValuePair.Value != null)
                    __result += Locale.GetOther("craftexample").Replace("<>", Locale.GetItem(keyValuePair.Value));

                __result += "\n";
            }

            if (recit.minimumCondition > 0f)
                __result = __result + Locale.GetOther("craftcondition")
                    .Replace("<>", (recit.minimumCondition * 100f).ToString("0.#")) + "\n";
        }

        return false;
    }
}