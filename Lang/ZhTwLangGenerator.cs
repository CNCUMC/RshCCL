using Bark.Base;

namespace RshLib.Lang;

public class ZhTwLangGenerator : ModLangGenBase
{
    protected override string LanguageCode => "zh-TW";
    protected override void BuildLocaleData()
    {
        Log("version", "RshCCL {0}，CUCoreLib 橋接已啟用，連線：{1}");
        Log("game_version_mismatch", "遊戲版本不符，預期：7.0.1，目前：{0}，將繼續載入");
        Log("hide", "已從插件登錄檔隱藏，防止誤判衝突。");
        Log("id_null", "嘗試註冊的物品 ID 為空！物品未註冊");
        Log("item_registred", "物品 {0} 之前已被註冊！物品未註冊");
        Log("sprite_null", "物品 {0} 的精靈為空");
        Log("info_null", "物品 {0} 的 ItemInfo 為空");
        Log("onspawn_callback_failed", "物品 '{0}' 的 onSpawn 回呼失敗：{1}");
        Log("refreshrecipelist_suppressed_exception", "RefreshRecipeList 中抑制的例外狀況：{0}：{1}");
        Log("loaded", "RshLib已加載！");
    }
}
