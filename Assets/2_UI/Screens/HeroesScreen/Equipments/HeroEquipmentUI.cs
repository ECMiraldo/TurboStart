using Persistence;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlotUI : UiSlot<Equipment>
{
    [field: SerializeField] public HeroScreenUI heroScreenUI { get; private set; }
    [field: SerializeField] public Sprite backgroundSprite { get; private set; }
    [field: SerializeField] public UiDragger dragger { get; private set; }
    [field: SerializeField] public EquipmentSlot slot { get; private set; }

    private void OnEnable()
    {
        heroScreenUI.onHeroChanged += OnHeroChanged;
        OnHeroChanged(heroScreenUI.GetCurrentHero());
    }

    private void OnDisable()
    {
        heroScreenUI.onHeroChanged -= OnHeroChanged;
    }

    private void OnHeroChanged(HeroData heroData)
    {
        SetItem(heroData.equipments[(int)slot]);
    }

    public override void RemoveItem()
    {
        currentItem = null;
        itemImage.sprite = backgroundSprite;
    }

    public override void OnDrop(PointerEventData eventData)
    {
        var parentSlot = eventData.pointerDrag.GetComponent<UiDragger>().parentObject.GetComponent<InventorySlot>();
        if (parentSlot.currentItem is Equipment equip)
        {
            EquipmentSO equipTemplate = equip.template<EquipmentSO>();
            if (equipTemplate.Slot == slot)
            {
                HeroData currentHero = heroScreenUI.GetCurrentHero();
                if (equipTemplate.CanEquip(currentHero))
                {
                    currentHero.equipments[(int)equipTemplate.Slot] = equip;
                    SaveLoadSystem.Instance.data.inventory.RemoveEntry(parentSlot.entry.index);
                    SetItem(equip); // equipment slot ui equip
                    parentSlot.SetItem(null); //inventory slot clear
                }
            }
        }
    }


}

