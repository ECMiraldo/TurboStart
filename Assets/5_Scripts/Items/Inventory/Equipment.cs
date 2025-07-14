using UnityEngine;
using System.Collections.Generic;

public abstract class Equipment : Item
{
    [field: SerializeField] public List<FixedAttributeModifier> Modifiers { get; private set; }
    [field: SerializeField] public EquipmentSlot Slot { get; private set; }
    [field: SerializeField] public EquipmentRarity Rarity { get; private set; }
    [field: SerializeField] public int Tier { get; private set; }

    public Equipment(
        string Name, 
        string SpriteName,
        int Tier,
        EquipmentSlot Slot, 
        EquipmentRarity Rarity, 
        List<FixedAttributeModifier> Modifiers) : base(Name, SpriteName, ItemType.Equipments)
    {
        this.Modifiers = Modifiers;
        this.Slot = Slot;
        this.Rarity = Rarity;
        this.Tier = Tier;
    }
}
