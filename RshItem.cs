using System;
using UnityEngine;

namespace RshLib;

public class RshItem
{
    public string baseItem;
    public ItemInfo info;
    public Action<GameObject, string> onSpawn;
    public Sprite sprite;
}