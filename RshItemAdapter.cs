using System;
using System.Collections.Generic;
using CUCoreLib.Data;
using CUCoreLib.Registries;
using UnityEngine;

namespace RshLib;

public static class RshItemAdapter
{
    internal static readonly Dictionary<string, Action<GameObject, string>> OnSpawnCallbacks = new();

    public static void RegisterItem(string itemId, RshItem rshItem)
    {
        if (string.IsNullOrEmpty(itemId))
            throw new ArgumentException(
                Plugin.LocaleLog("id_null",
                    "The id of item you're trying to register is null or empty! Item wasn't registred."));

        if (ItemRegistry.TryGetCustomInfo(itemId, out _))
            throw new ArgumentException("item_registred",
                Plugin.LocaleLog("Item {0} already was registred before! Item wasn't registred.", itemId));

        if (null == rshItem.sprite)
            Plugin.LogWarning("sprite_null", "The sprite of item {0} is null", itemId);

        if (null == rshItem.info)
            Plugin.LogWarning("info_null", "The info of item {0} is null", itemId);

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
                // ignored
            }
        }

        return custom;
    }
}

internal class RshSpawnCallback : MonoBehaviour
{
    private void Start()
    {
        if (!TryGetComponent<Item>(out var item))
        {
            Destroy(this);
            return;
        }

        var fullId = item.id;
        var suffixIndex = fullId.IndexOf('$');
        var baseId = suffixIndex > 0
            ? fullId.Substring(0, suffixIndex)
            : fullId;
        var suffix = suffixIndex > 0
            ? fullId.Substring(suffixIndex + 1)
            : "";

        if (RshItemAdapter.OnSpawnCallbacks.TryGetValue(baseId, out var callback))
        {
            try
            {
                callback(gameObject, suffix);
            }
            catch (Exception ex)
            {
                Plugin.LogError("onspawn_callback_failed", "onSpawn callback failed for item '{0}': {1}", baseId, ex);
            }
        }

        Destroy(this);
    }
}