using NaughtyAttributes;
using UnityEngine;
using Newtonsoft.Json;
using System;


[CreateAssetMenu(menuName = "Items/Misc")]
public class ItemSO : IDScriptableObject
{
    [Header("Base Item")]
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public int BuyPrice { get; private set; }
    [field: SerializeField] public int SellPrice { get; private set; }
    [field: SerializeField, ReadOnly] public virtual bool IsStackable { get; } = true;

    public virtual Item ToItem() => new Item(this.id);
    public virtual string GetFullDescription() => Description;
}

[Serializable]
public class Item : IDraggable
{
    public readonly string SoId;

    [JsonIgnore] public Sprite Sprite => SO<ItemSO>().Sprite;

    public Item(string SOId)
    {
        this.SoId = SOId;
    }


    public T SO<T>() where T : ItemSO => Database.items[SoId] as T;

    public virtual string GetFullDescription()
    {
        return $"{SO<ItemSO>().Description}\n\n";
    }
}

