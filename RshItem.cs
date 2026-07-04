using System;
using UnityEngine;

namespace RshLib;

public class RshItem
{
    public Sprite sprite;
    public ItemInfo info;
    public Action<GameObject, string> onSpawn;
    public string baseItem;
}