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
   
    private void Start() 
    {
        OnEntryChanged();
    }
    private void OnEnable()
    {
        entry = SaveLoadSystem.Instance.data.inventory.items[transform.GetSiblingIndex()];
        entry.OnEntryChanged += OnEntryChanged;
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
        currentHero.equipments[(int)equip.template<EquipmentSO>().Slot] = null;
        parentSlot.SetItem(null);
        if (currentItem == null) entry.SetEntry(equip, 1);
        else SaveLoadSystem.Instance.data.inventory.AddItem(equip);
    }

    private void EvaluateItemRules()
    {
        itemDragger.enabled = true;
        itemImage.color = Color.white;

        HeroData currentHero = heroScreenUI != null ? heroScreenUI.GetCurrentHero() : null;

        if (currentHero == null || currentItem == null)
        {
            return;
        }

        if (currentItem is Equipment equipment)
        {
            bool canEquip = equipment.template<EquipmentSO>().CanEquip(currentHero);
            itemDragger.enabled = canEquip;
            itemImage.color = canEquip ? Color.white : new Color(1f, 0.3f, 0.3f, 1f);
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
    }
}
