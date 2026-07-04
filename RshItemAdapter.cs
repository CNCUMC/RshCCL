using System;
using System.Collections.Generic;
using CUCoreLib.Data;
using CUCoreLib.Registries;
using UnityEngine;

namespace RshLib;

/// <summary>
/// RshLib → CUCoreLib 的注册转接器。
/// 将老模组调用的 <see cref="RshItem"/> 转换为 <see cref="CustomItemInfo"/>，
/// 并通过 <see cref="ItemRegistry.Register"/> 注册到 CCL。
/// </summary>
public static class RshItemAdapter
{
    /// <summary>存储 itemId → onSpawn 回调的映射，供 <see cref="RshSpawnCallback"/> 查找。</summary>
    internal static readonly Dictionary<string, Action<GameObject, string>> OnSpawnCallbacks =
        new Dictionary<string, Action<GameObject, string>>();

    /// <summary>
    /// 将 RshItem 转接注册到 CUCoreLib。
    /// 保持与原 RshLib 相同的验证行为和异常消息。
    /// </summary>
    public static void RegisterItem(string itemId, RshItem rshItem)
    {
        if (string.IsNullOrEmpty(itemId))
            throw new ArgumentException("The id of item you're trying to register is null or empty! Item wasn't registred.");

        if (ItemRegistry.TryGetCustomInfo(itemId, out _))
            throw new ArgumentException($"Item {itemId} already was registred before! Item wasn't registred.");

        if (null == rshItem.sprite)
            Plugin.Logger.LogWarning($"[RshCCL] The sprite of item {itemId} is null");

        if (null == rshItem.info)
            Plugin.Logger.LogWarning($"[RshCCL] The info of item {itemId} is null");

        if (string.IsNullOrEmpty(rshItem.baseItem))
            rshItem.baseItem = "geofruit";

        // 转换为 CCL 的 CustomItemInfo
        var customInfo = ConvertToCustomItemInfo(rshItem);

        // 处理 onSpawn 回调：注册到 SpawnComponents
        if (rshItem.onSpawn != null)
        {
            OnSpawnCallbacks[itemId] = rshItem.onSpawn;
            customInfo.SpawnComponents ??= [];
            customInfo.SpawnComponents.Add(typeof(RshSpawnCallback).AssemblyQualifiedName);
        }

        // 通过 CCL 注册
        ItemRegistry.Register(itemId, customInfo, rshItem.sprite);
    }

    /// <summary>
    /// 将 <see cref="RshItem.info"/> 复制到新的 <see cref="CustomItemInfo"/> 实例。
    /// 如果 info 本身就是 CustomItemInfo，则直接返回。
    /// </summary>
    private static CustomItemInfo ConvertToCustomItemInfo(RshItem rshItem)
    {
        if (rshItem.info is CustomItemInfo existing)
            return existing;

        var custom = new CustomItemInfo();
        var source = rshItem.info;

        if (source == null) return custom;
        // 复制 ItemInfo 继承链上的所有公共实例字段
        foreach (var field in typeof(ItemInfo).GetFields(
                     System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
        {
            try
            {
                var value = field.GetValue(source);
                field.SetValue(custom, value);
            }
            catch
            {
                // 跳过无法设置的字段（如只读字段）
            }
        }

        return custom;
    }
}

/// <summary>
/// 通过 CCL SpawnComponents 机制在物品生成时触发 RshLib 的 onSpawn 回调。
/// 在 Start() 中执行回调后自动销毁自身。
/// </summary>
internal class RshSpawnCallback : MonoBehaviour
{
    private void Start()
    {
        if (!TryGetComponent<Item>(out var item))
        {
            Destroy(this);
            return;
        }

        // 处理 "$" 后缀：itemId$suffix → baseId=itemId, suffix=suffix
        var fullId = item.id;
        var suffixIndex = fullId.IndexOf('$');
        var baseId = suffixIndex > 0 ? fullId.Substring(0, suffixIndex) : fullId;
        var suffix = suffixIndex > 0 ? fullId.Substring(suffixIndex + 1) : "";

        if (RshItemAdapter.OnSpawnCallbacks.TryGetValue(baseId, out var callback))
        {
            try
            {
                callback(gameObject, suffix);
            }
            catch (Exception ex)
            {
                Plugin.Logger.LogError($"[RshCCL] onSpawn callback failed for item '{baseId}': {ex}");
            }
        }

        Destroy(this);
    }
}