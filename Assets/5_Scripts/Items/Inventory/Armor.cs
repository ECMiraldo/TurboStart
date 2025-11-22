using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class Armor : Equipment
{
    [field: SerializeField] public ArmorType ArmorType { get; private set; }

    public override Sprite Sprite
    {
        get
        {
            if (_sprite != null) return _sprite;
            else _sprite = Resources.Load<Sprite>($"Items/Tier{Tier}/Armors/{Slot}/{ArmorType}/{Rarity}/{SpriteName}");
            return _sprite;
        }
    }

    public Armor(
        string Name,
        string SpriteName,
        int Tier,
        EquipmentSlot Slot,
        EquipmentRarity Rarity,
        ArmorType ArmorType,
        List<AttributeModifier> Modifiers) : base(Name, SpriteName, Tier, Slot, Rarity, Modifiers)
    {
        this.ArmorType = ArmorType;
    }
}
