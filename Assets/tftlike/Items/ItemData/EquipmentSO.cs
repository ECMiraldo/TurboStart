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
    [field: SerializeField] public int level { get; private set; }
    [field: SerializeField] public int enchantSlots { get; private set; }
    [field: SerializeField] public List<ItemEffect> effects {get; private set;}
    [field: SerializeField] public List<AttributeModifier> modifiers {get; private set;}
    [field: SerializeField] public ItemRarity rarity {get; private set; }
    //[field: SerializeField] public List<EnchantmentItem> enchantItems { get; private set; }

    public Equipment(EquipmentSO equipmentSO) : base(equipmentSO.id)
    {
        //SaveLoadSystem.Instance.data.heroes.Sum((x) => x.heroLevelData.level);
        level = equipmentSO.level;
        rarity = equipmentSO.equipmentArchetype.RollRarity(equipmentSO);
        effects = equipmentSO.equipmentArchetype.RollEffects(level, rarity);
        modifiers = equipmentSO.equipmentArchetype.RollStats(level, rarity);
        enchantSlots = equipmentSO.EnchantmentSlots;
    }

    [JsonConstructor]
    public Equipment(
        int level,
        int enchantSlots,
        List<ItemEffect> effects,
        List<AttributeModifier> modifiers,
        ItemRarity rarity,
        string SoId
        ): base(SoId)
    {
        this.level = level;
        this.enchantSlots = enchantSlots;
        this.effects = effects;
        this.modifiers = modifiers;
        this.rarity = rarity;
    }


    public void Equip(HeroData heroData, InventorySlot formerSlot)
    {
        HeroEquipmentData heroEquipmentData = heroData.equipmentData;
        heroEquipmentData.equipments[(int)SO<EquipmentSO>().Slot] = this;
        SaveLoadSystem.Instance.data.inventory.RemoveEntry(formerSlot.entry.index);
    }
    public void Unequip(HeroData heroData)
    {
        //little bit spaghetti here but backend data of inventory is dealt on the inventory slot
        HeroEquipmentData heroEquipmentData = heroData.equipmentData;
        heroEquipmentData.equipments[(int)SO<EquipmentSO>().Slot] = null;
    }

    public string GetFullName()
    {
        // if (upgradeLevel > 0) return SO<EquipmentSO>().Name + " +" + upgradeLevel.ToString() + $" [{enchantSlots}]";
        // else 
        return SO<EquipmentSO>().Name;
    }

    public virtual void Upgrade()
    {
        //upgradeModifier.ChangeValue(upgradeModifier.Value + Mathf.Floor(upgradeLevel / 2));
    }

    //public void Enchant(EnchantmentItem enchantmentItem)
    //{
    //    enchantItems.Add(enchantmentItem);
    //}

    public override string GetFullDescription()
    {
        EquipmentSO so = SO<EquipmentSO>();
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


