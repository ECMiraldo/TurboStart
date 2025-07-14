using UnityEngine;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable]
public class Inventory 
{
    public event Action<Item> onItemAdded;

    [field: SerializeField] public List<InventoryEntry> items { get; private set; }

    [JsonConstructor]
    public Inventory(List<InventoryEntry> items)
    {
        this.items = items;
    }

    public Inventory() {
        items = new();
        for (int j = 0; j < 50; j++)
        {
            items.Add(new InventoryEntry(j));
        }
        
    }

    public bool AddItem(Item item, int quantity = 1)
    {
        int index = FindFirstEmptySlot();
        if (index != -1)
        {
            items[index].SetEntry(item, quantity);
            onItemAdded?.Invoke(item);
            return true;
        }
        return false;
    }

    public bool RemoveItem(int index)
    {
        items[index].SetEntry(null, 0);
        return true;
    }

    public bool RemoveItem(Item item, int quantity = 1)
    {
        InventoryEntry entry = items.Find((x) => x.item != null && !string.IsNullOrEmpty(x.item.Id) && x.item.Id == item.Id);

        if (entry.quantity < quantity) return false;
        entry.quantity -= quantity;
        return true;
    }
    private int FindFirstEmptySlot()
    {
        var slot = items.Find((x) => x.item == null || string.IsNullOrEmpty(x.item.Id));
        if (slot != null) return slot.index;
        else return -1;
    }

}
