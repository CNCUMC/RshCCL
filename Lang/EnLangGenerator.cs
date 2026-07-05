using Bark.Base;

namespace RshLib.Lang;

public class EnLangGenerator : ModLangGenBase
{
    protected override string LanguageCode => "EN";
    protected override void BuildLocaleData()
    {
        Log("version", "RshCCL {0}, CUCoreLib bridge active, Together: {1}");
        Log("game_version_mismatch", "GAME VERSION MISMATCH, Expected: 7.0.1, Current: {0}, Loading will continue");
        Log("id_null", "The id of item you're trying to register is null or empty! Item wasn't registered.");
        Log("item_registred", "Item {0} was already registered before! Item wasn't registered.");
        Log("sprite_null", "The sprite of item {0} is null");
        Log("info_null", "The info of item {0} is null");
        Log("onspawn_callback_failed", "onSpawn callback failed for item '{0}': {1}");
        Log("refreshrecipelist_suppressed_exception", "Suppressed exception in RefreshRecipeList: {0}: {1}");
        Log("savegame_duplicate_key_suppressed", "Suppressed duplicate key in SaveGame: {0}");
        Log("hide", "Hidden from plugin registry to prevent false conflict detection.");
        Log("bark_init_failed", "Failed to initialize Bark localization: {0}");
        Log("restore", "Restored to plugin registry after all mods loaded.");
        Log("loaded", "RshCCL loaded!");
    }
}
