using Newtonsoft.Json;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum EquipmentSlot : byte
{
    Head = 0,
    Body = 1,
    Boots = 2,
    Weapon = 3,
}

public enum ArmorType : byte
{
    Cloth = 0,
    Leather = 1,
    Plate = 2,
}


[CreateAssetMenu(menuName = "Items/Equipment")]
public class EquipmentSO : ItemSO
{
    [field: Space(25)]
    [field: Header("Equipment")]
    [field: SerializeField] public int level { get; private set; }
    [field: SerializeField] public virtual EquipmentSlot Slot { get; private set; }
    [field: SerializeField] public ArmorType armorType { get; private set; }
    [field: SerializeField] public int EnchantmentSlots { get; private set; }
    [field: SerializeField] public EquipmentArchetypeSO equipmentArchetype {get; private set;}

   // [field: SerializeField] public List<UpgradeRequirement> upgradeRequirements = new(Constants.MAX_UPGRADE_LEVEL);
    public override bool IsStackable => false;
    public override Item ToItem() => new Equipment(this);
    public virtual bool CanEquip(HeroData characterData)
    {
        HeroTemplateSO hero = characterData.template;
        return hero.usableArmors.Contains(armorType);
    }


    public override string GetFullDescription()
    {
        return base.GetFullDescription();
    }

}





public class Equipment : Item
{
    public int level;
    public int enchantSlots;
    public List<ItemEffect> effects;
    public List<AttributeModifier> modifiers;
    public ItemRarity rarity;

    //[field: SerializeField] public List<EnchantmentItem> enchantItems { get; private set; }
    [JsonConstructor] public Equipment() : base() { }
    public Equipment(EquipmentSO equipmentSO) : base(equipmentSO.id)
    {
        //SaveLoadSystem.Instance.data.heroes.Sum((x) => x.heroLevelData.level);
        level = equipmentSO.level;
        rarity = equipmentSO.equipmentArchetype.RollRarity(equipmentSO);
        effects = equipmentSO.equipmentArchetype.RollEffects(level, rarity);
        modifiers = equipmentSO.equipmentArchetype.RollStats(level, rarity);
        enchantSlots = equipmentSO.EnchantmentSlots;
    }

    public string GetFullName()
    {
        // if (upgradeLevel > 0) return SO<EquipmentSO>().Name + " +" + upgradeLevel.ToString() + $" [{enchantSlots}]";
        // else 
        return template<EquipmentSO>().Name;
    }

    public override string GetFullDescription()
    {
        EquipmentSO so = template<EquipmentSO>();
        string text = $"{so.Description}\n\n\n ";
        //foreach (AttributeModifier modifier in addedModifiers)
        //{
        //    text += modifier.ToString() + "\n";
        //}
        text += $"\nSlot: {so.Slot}\n\n";
        text += $"RequiredLevel: {so.level}\n\n";
        //text += $"Weight: {Weight}";
        return text;
    }

}


