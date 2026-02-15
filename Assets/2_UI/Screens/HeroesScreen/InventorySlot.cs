using Persistence;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : UiSlot<Item>
{
    [field: SerializeField] private TextMeshProUGUI quantityText;
    public InventoryEntry entry { get; private set; }

    private void Start()
    {
        entry = SaveLoadSystem.Instance.data.inventory.items[transform.GetSiblingIndex()];
    }
    private void OnEnable()
    {
        entry.OnEntryChanged += OnEntryChanged;
        OnEntryChanged();
    }

    private void OnDisable()
    {
        entry.OnEntryChanged -= OnEntryChanged;
    }

    private void OnEntryChanged()
    {
        if (entry == null || entry.quantity == 0) RemoveItem();
        SetItem(entry.item);
        quantityText.text = entry.quantity.ToString();
    }

    public override void OnDrop(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }




}
