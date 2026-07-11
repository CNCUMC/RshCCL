using CUCoreLib.Registries;
using HarmonyLib;

namespace RshLib.Patchers;

[HarmonyPatch(typeof(ConsoleScript))]
internal class ConsoleScriptPatch
{
    [HarmonyPatch("RegisterSpawnEntities")]
    private static void Postfix(ConsoleScript __instance)
    {
        var command = ConsoleScript.SearchExact("spawn");
        if (command == null) return;

        foreach (var itemId in ItemRegistry.GetRegisteredItemIds())
            if (!command.argAutofill[0].Contains(itemId))
                command.argAutofill[0].Add(itemId);
    }
}