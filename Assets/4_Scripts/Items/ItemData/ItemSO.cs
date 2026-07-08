using NaughtyAttributes;
using UnityEngine;
using Newtonsoft.Json;
using System;

public enum ItemRarity : byte
{
    Normal = 0,
    Uncommon = 1,
    Rare = 2,
    Epic = 3,
    Legendary = 4,
}

[CreateAssetMenu(menuName = "Items/Misc")]
public class ItemSO : IDScriptableObject
{
    [Header("Base Item")]
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField, ReadOnly] public virtual bool IsStackable { get; } = true;
    [field: SerializeField] public ItemRarity[] rarities {get; private set;} = {ItemRarity.Normal};

    public virtual Item ToItem() => new Item(this.id);
    public virtual string GetFullDescription() => Description;
}

[Serializable]
public class Item : IDraggable
{
    public readonly string SoId;
    [JsonIgnore] public Sprite Sprite => template<ItemSO>().Sprite;


    [JsonConstructor] public Item() {}
    public Item(string SOId)
    {
        this.SoId = SOId;
    }


    public T template<T>() where T : ItemSO => Database.items[SoId] as T;

    public virtual string GetFullDescription()
    {
        return $"{template<ItemSO>().Description}\n\n";
    }
}

