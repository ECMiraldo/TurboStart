using UnityEngine;
using System;
using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

[Serializable]
public class Inventory
{
    public event Action<ItemSO, int> onItemAdded;
    public event Action<ItemSO, int> onItemRemoved;

    [field: SerializeField] public List<InventoryEntry> items { get; private set; }

    [JsonConstructor]
    public Inventory(List<InventoryEntry> items)
    {
        this.items = items;
    }

    public Inventory()
    {
        int maxItems = 100;
        items = new(maxItems);
        //initialize dictionary
        for (int i = 0; i < items.Capacity; i++)
        {
            items.Add(new InventoryEntry(i));
        }
    }

    public void SwapEntries(int a, int b)
    {
        Item item = items[a].item;
        int qnt = items[a].quantity;
        items[a].SetEntry(items[b].item, items[b].quantity);
        items[b].SetEntry(item, qnt);
    }

    public int GetQuantity(Item item)
    {
        return items.FindAll((x) => x.item != null && !string.IsNullOrEmpty(x.item.SoId) && x.item.SoId == item.SoId).Sum((x) => x.quantity);
    }
    public int GetQuantity(ItemSO item)
    {
        return items.FindAll((x) => x.item != null && !string.IsNullOrEmpty(x.item.SoId) && x.item.SoId == item.id).Sum((x) => x.quantity);
    }

    public bool HasQuantity(ItemSO item, int quantity)
    {
        return items.FindAll((x) => x.item != null && !string.IsNullOrEmpty(x.item.SoId) && x.item.SoId == item.id).Sum((x) => x.quantity) > quantity;
    }

    public bool AddItem(Item item, int quantity = 1)
    {
        ItemSO itemSO = item.SO<ItemSO>();
        //checks for full inventory
        if (!itemSO.IsStackable)
        {
            int index = FindFirstEmptySlot();
            if (index != -1)
            {
                items[index].SetEntry(item, 1);
                onItemAdded?.Invoke(itemSO, quantity);
            }
            return index != -1;
        }
        var entry = items.Find((x) => x.item != null && !string.IsNullOrEmpty(x.item.SoId) && x.item.SoId == item.SoId);
        if (entry == null) //no entry to add item in
        {
            int index = FindFirstEmptySlot();
            if (index != -1)
            {
                items[index].SetEntry(item, quantity);
                onItemAdded?.Invoke(itemSO, quantity);
                return true;
            }
        }
        else
        {
            items[entry.index].ChangeQuantity(entry.quantity + quantity);
            onItemAdded?.Invoke(itemSO, quantity);
            return true;
        }
        return false;
    

    }

    public bool RemoveEntry(int index)
    {
        items[index].SetEntry(null, 0);
        return true;
    }

    public bool RemoveItem(ItemSO item, int quantity = 1)
    {
        InventoryEntry entry = items.Find((x) => x.item != null && !string.IsNullOrEmpty(x.item.SoId) && x.item.SoId == item.id);
        if (entry.quantity < quantity) return false;

        if (entry.quantity == quantity) RemoveEntry(entry.index);
        else entry.ChangeQuantity(entry.quantity - quantity);

        onItemRemoved?.Invoke(item, quantity);
        return true;
    }

    private int FindFirstEmptySlot()
    {
        var slot = items.Find((x) => x.item == null || string.IsNullOrEmpty(x.item.SoId));
        if (slot != null) return slot.index;
        else return -1;
    }

}
