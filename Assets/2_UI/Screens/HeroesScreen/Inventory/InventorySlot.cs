using Persistence;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : UiSlot<Item>
{
    [field: SerializeField] private TextMeshProUGUI quantityText;
    [field: SerializeField] public HeroScreenUI heroScreenUI;
    public InventoryEntry entry { get; private set; }
   
    private void OnEnable()
    {
        entry = SaveLoadSystem.Instance.data.inventory.items[transform.GetSiblingIndex()];
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
        var parentSlot = eventData.pointerDrag.GetComponent<UiDragger>().parentObject.GetComponent<EquipmentSlotUI>();
        if (!parentSlot) return;
        HeroData currentHero = heroScreenUI.GetCurrentHero();
        Equipment equip = parentSlot.currentItem;
        equip.Unequip(currentHero); // "backend" unequip
        parentSlot.SetItem(null);
        if (currentItem == null) entry.SetEntry(equip, 1);
        else SaveLoadSystem.Instance.data.inventory.AddItem(equip);
    }

   




}
