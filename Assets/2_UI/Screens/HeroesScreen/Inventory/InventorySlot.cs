using Persistence;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


public class InventorySlot : UiSlot<Item>, IPointerEnterHandler, IPointerExitHandler
{
    [field: SerializeField] private TextMeshProUGUI quantityText;
    [field: SerializeField] public HeroScreenUI heroScreenUI;
    [field: SerializeField] public UiDragger itemDragger;
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
        EvaluateItemRules();
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

    private void EvaluateItemRules()
    {
        itemDragger.enabled = true;
        itemImage.color = Color.white;

        HeroData currentHero = heroScreenUI.GetCurrentHero();

        if (currentHero == null || currentItem == null)
        {
            return;
        }

        if (currentItem is Equipment equipment)
        {
            itemDragger.enabled = equipment.SO<EquipmentSO>().CanEquip(currentHero);
            itemImage.color = new Color(1f, 0.3f, 0.3f, 1f);
        }

    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (entry == null || entry.item == null)
            return;

        if (eventData.button != PointerEventData.InputButton.Right)
            return;

        ItemDetailsUI.Instance.Show(entry.item, eventData.position);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (ItemDetailsUI.Instance.IsOpen)
        {
            ItemDetailsUI.Instance.Close();
        }
    }
    

}
