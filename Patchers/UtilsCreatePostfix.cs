using System;
using HarmonyLib;
using UnityEngine;

namespace RshLib.Patchers;

[HarmonyPatch(typeof(Utils))]
internal class UtilsCreatePostfix
{
    [HarmonyPatch("Create", typeof(string), typeof(Vector2), typeof(float))]
    [HarmonyPostfix]
    private static void PostfixVector2(string id, ref GameObject __result)
    {
        if (__result == null) return;
        InvokeOnSpawn(__result, id);
    }

    [HarmonyPatch("Create", typeof(string), typeof(Transform))]
    [HarmonyPostfix]
    private static void PostfixTransform(string id, ref GameObject __result)
    {
        if (__result == null) return;
        InvokeOnSpawn(__result, id);
    }

    private static void InvokeOnSpawn(GameObject go, string id)
    {
        var suffixIndex = id.IndexOf('$');
        var baseId = suffixIndex > 0
            ? id.Substring(0, suffixIndex)
            : id;
        var suffix = suffixIndex > 0
            ? id.Substring(suffixIndex + 1)
            : "";

        if (!RshItemAdapter.OnSpawnCallbacks.TryGetValue(baseId, out var callback))
            return;

        try
        {
            callback(go, suffix);
        }
        catch (Exception ex)
        {
            Plugin.LogError("[UtilsCreatePostfix] onSpawn for '{0}' failed: {1}", baseId, ex);
        }
    }
}