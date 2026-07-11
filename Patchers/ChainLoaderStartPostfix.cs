using BepInEx.Bootstrap;
using HarmonyLib;

namespace RshLib.Patchers;

[HarmonyPatch(typeof(Chainloader))]
internal class ChainLoaderStartPostfix
{
    [HarmonyPatch("Start")]
    [HarmonyPostfix]
    private static void Postfix()
    {
        Plugin.RestoreToPluginInfos();
    }
}