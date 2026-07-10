using HarmonyLib;

namespace RshLib.Patchers;

[HarmonyPatch(typeof(BepInEx.Bootstrap.Chainloader))]
internal class ChainLoaderStartPostfix
{
    [HarmonyPatch("Start")]
    [HarmonyPostfix]
    private static void Postfix()
    {
        Plugin.RestoreToPluginInfos();
    }
}