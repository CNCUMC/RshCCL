using Bark.Base;

namespace RshLib.Lang;

public class ZhCnLangGenerator : ModLangGenBase
{
    protected override string LanguageCode => "zh-CN";
    protected override void BuildLocaleData()
    {
        Log("version", "RshCCL {0}，CUCoreLib 桥接已激活，联机：{1}");
        Log("game_version_mismatch", "游戏版本不匹配，预期：7.0.1，当前：{0}，将继续加载");
        Log("hide", "已从插件注册表隐藏，防止误判冲突。");
        Log("id_null", "尝试注册的物品 ID 为空！物品未注册");
        Log("item_registred", "物品 {0} 之前已被注册！物品未注册");
        Log("sprite_null", "物品 {0} 的精灵为空");
        Log("info_null", "物品 {0} 的 ItemInfo 为空");
        Log("onspawn_callback_failed", "物品 '{0}' 的 onSpawn 回调失败：{1}");
        Log("refreshrecipelist_suppressed_exception", "RefreshRecipeList 中抑制的异常：{0}：{1}");
        Log("loaded", "RshCCL 已加载！");
    }
}
