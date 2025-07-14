using UnityEngine;
using System;
using Newtonsoft.Json;

public abstract class Item : IDraggable
{
    [JsonProperty][field: SerializeField] public string Name { get; private set; }
    [JsonProperty][field: SerializeField] public string Id { get; private set; }

    [JsonProperty][field: SerializeField] public string SpriteName { get; private set; }
    [JsonProperty][field: SerializeField] public ItemType ItemType { get; private set; }

    [JsonIgnore] protected Sprite _sprite;
    [JsonIgnore]
    public abstract Sprite Sprite { get; }

    public Item(string Name, string SpriteName, ItemType ItemType)
    {
        this.Name = Name;
        this.SpriteName = SpriteName;
        this.ItemType = ItemType;
        this.Id = Guid.NewGuid().ToString();
    }
}
