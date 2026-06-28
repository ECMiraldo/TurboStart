using UnityEngine;
using System;

[Serializable]
public class ShopEntry
{
    public event Action OnEntryChanged;

    [field: SerializeField] public Item item;
    [field: SerializeField] public int quantity;
    [field: SerializeField] public int index;

    public ShopEntry(int index)
    {
        this.index = index;
        this.item = null;
        this.quantity = 0;
    }

    public void SetEntry(Item item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
        OnEntryChanged?.Invoke();
    }
    public void ChangeQuantity(int quantity)
    {
        this.quantity = quantity;
        OnEntryChanged?.Invoke();
    }

    public void RemoveItem()
    {
        this.item = null;
        this.quantity = 0;
        OnEntryChanged?.Invoke();
    }
}
