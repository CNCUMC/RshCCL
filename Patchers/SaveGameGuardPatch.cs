using System;
using HarmonyLib;

namespace RshLib.Patchers;

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