using System;
using HarmonyLib;

namespace RshLib.Patchers;

[HarmonyPatch(typeof(PlayerCamera))]
internal class RefreshRecipeListGuardPatch
{
    [HarmonyPatch("RefreshRecipeList")]
    [HarmonyFinalizer]
    public static Exception Finalizer(Exception __exception)
    {
        if (__exception == null) return null;
        Plugin.LogWarning("refreshrecipelist_suppressed_exception",
            "Suppressed exception in RefreshRecipeList: {0}: {1}", __exception.GetType().Name, __exception.Message);
        return null;
    }
}