using UnityEngine.EventSystems;

public class InventorySlot : UiSlot<Item>
{

    private InventoryEntry entry;
    private void Start()
    {
        entry = GameManager.ProfileData.inventory.items[transform.GetSiblingIndex()];
    }
    private void OnEnable()
    {
        SetItem(entry.item, entry.quantity.ToString());
        entry.OnEntryChanged += OnEntryChanged;
    }

    private void OnDisable()
    {
        entry.OnEntryChanged -= OnEntryChanged;
    }

    private void OnEntryChanged()
    {
        if (entry == null || entry.quantity == 0) RemoveItem();
        SetItem(entry.item, entry.quantity.ToString());
    }

    public override void OnDrop(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }




}
