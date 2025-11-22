
using System.Collections.Generic;

public enum ItemType : byte
{
    Equipments,
    Enchantments,
    Misc,
}

public enum EquipmentSlot : byte
{
    UpperHead = 0, Necklace = 1, Shoulders = 2, Gloves = 3, Bracers =  4, Body = 5,
    Pants = 6, MainHand = 7, OffHand = 8, Boots = 9, 
}


public enum ArmorType : byte
{
    cloth,
    leather,
    mail,
    Shield,
    Quiver,
    Focus,
    OffHandWeapon
}

public enum WeaponType : byte
{
    dagger,
    sword,
    axe,
    spear,
    mace,
    bow,
    crossbow,
}

public enum EquipmentRarity : byte
{
    common,
    uncommon,
    rare,
    epic,
    legendary
}


public enum CombatStats : byte
{
    MATK,
    RATK,
    DEF,
    INT,
    MDEF,
    HEALTH,
    MANA,
    INI,
    CRIT,
    CRITDMG,
    EVA,
    ACC,
}

public enum Element : byte
{
    Water,
    Fire,

}

public static class Constants
{
    public const string HERO_SPRITES_ADDRESS = "Heroes/HeroSprites";
 
}
