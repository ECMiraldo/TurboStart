using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class Weapon : Equipment
{
    [field: SerializeField] public WeaponType WeaponType { get; private set; }

    public override Sprite Sprite
    {
        get
        {
            if (_sprite != null) return _sprite;
            else _sprite = Resources.Load<Sprite>($"Items/Tier{Tier}/Weapons/{WeaponType}/{Rarity}/{SpriteName}");
            return _sprite;
        }
    }

    public Weapon(
        string Name, 
        string SpriteName, 
        int Tier,
        EquipmentSlot Slot, 
        EquipmentRarity Rarity,
        WeaponType weaponType,
        List<AttributeModifier> Modifiers) : base(Name, SpriteName, Tier, Slot, Rarity, Modifiers)
    {
        this.WeaponType = weaponType;
    }
}
