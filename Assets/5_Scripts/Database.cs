using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtils;

[DefaultExecutionOrder(-10)]
public class Database : Singleton<Database>
{
    public List<string> HeroSpriteNames = new List<string>();


    protected override void Awake()
    {
        base.Awake();
        var items = Resources.LoadAll<Texture2D>(Constants.HERO_SPRITES_ADDRESS);
        foreach (var item in items)
        {
            HeroSpriteNames.Add(item.name);
        }
    }
    private void LoadResource<T>(SerializedDictionary<string, T> dict, string folder) where T : IDScriptableObject
    {
        var items = Resources.LoadAll<T>(folder);
        foreach (var item in items)
        {
            dict.Add(item.id, item);
        }
    }
}
