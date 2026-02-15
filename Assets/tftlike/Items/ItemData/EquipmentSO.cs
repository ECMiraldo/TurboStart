using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentSlot : byte
{
    Head = 0,
    Body = 1,
    Pants = 2,
    Boots = 3,
    MainHand = 4,
    Offhand = 5,
    Cape = 6

}


public  class EquipmentSO : ItemSO
{
    [field: Space(25)]
    [field: Header("Equipment")]
    [field: SerializeField] public int RequiredLevel { get; private set; }
    [field: SerializeField] public virtual EquipmentSlot Slot { get; private set; }
    [field: SerializeField] public int EnchantmentSlots { get; private set; }
    //  [field: SerializeField] public List<ClassDataSO> usableClasses {get; private set;}
     //[field: SerializeField] public List<RollableAttributeModifier> attributeModifiers { get; private set; }
   // [field: SerializeField] public List<DamageModifierSO> offensiveDamageModifiers { get; private set; }
   // [field: SerializeField] public List<DamageModifierSO> defensiveDamageModifiers { get; private set; }


   // [field: SerializeField] public List<UpgradeRequirement> upgradeRequirements = new(Constants.MAX_UPGRADE_LEVEL);
    public override bool IsStackable => false;
    public virtual bool CanEquip(HeroData characterData)
    {
        return true;

    }

    public override string GetFullDescription()
    {
        return base.GetFullDescription();
    }

}





public class Equipment : Item
{
    //[field: SerializeField] public List<FixedAttributeModifier> baseModifiers { get; private set; }
    //[field: SerializeField] public List<FixedAttributeModifier> addedModifiers { get; private set; }
    [field: SerializeField] public int upgradeLevel { get; private set; }
    [field: SerializeField] public int enchantSlots { get; private set; }
    //[field: SerializeField] public List<EnchantmentItem> enchantItems { get; private set; }
    //[field: SerializeField] public FixedAttributeModifier upgradeModifier { get; protected set; }


    public Equipment(EquipmentSO equipmentSO) : base(equipmentSO.id)
    {
        //baseModifiers = new();
        //addedModifiers = new();
        //enchantItems = new();
        upgradeLevel = 0;
        enchantSlots = equipmentSO.EnchantmentSlots;

        //foreach (RollableAttributeModifier mod in equipmentSO.attributeModifiers)
        //{
        //    addedModifiers.Add(new FixedAttributeModifier(mod));
        //}
    }

    [JsonConstructor]
    public Equipment(
        //List<FixedAttributeModifier> baseModifiers,
        //List<FixedAttributeModifier> addedModifiers,
        int upgradeLevel,
        int enchantSlots,
        //List<EnchantmentItem> enchantItems,
        string SoId
        //FixedAttributeModifier upgradeModifier)
        ): base(SoId)
    {
        //this.baseModifiers = baseModifiers;
        //this.addedModifiers = addedModifiers;
        this.enchantSlots = enchantSlots;
        this.upgradeLevel = upgradeLevel;
        //this.enchantItems = enchantItems;
        //this.upgradeModifier = upgradeModifier;
    }

    public string GetFullName()
    {
        if (upgradeLevel > 0) return SO<EquipmentSO>().Name + " +" + upgradeLevel.ToString() + $" [{enchantSlots}]";
        else return SO<EquipmentSO>().Name;
    }

    public virtual void Upgrade()
    {
        upgradeLevel++;
        //upgradeModifier.ChangeValue(upgradeModifier.Value + Mathf.Floor(upgradeLevel / 2));
    }

    //public void Enchant(EnchantmentItem enchantmentItem)
    //{
    //    enchantItems.Add(enchantmentItem);
    //}

    public void OnEquip(UnitStats stats)
    {
        EquipmentSO so = SO<EquipmentSO>();

        //foreach (AttributeModifier mod in baseModifiers)
        //{
        //    stats.stats[mod.StatDefinition].AddModifier(mod);
        //}
        //foreach (AttributeModifier mod in addedModifiers)
        //{
        //    stats.stats[mod.StatDefinition].AddModifier(mod);
        //}
        //foreach (DamageModifierSO dmgMod in so.offensiveDamageModifiers)
        //{
        //    stats.offensiveModifiers.Add(dmgMod);
        //}
        //foreach (DamageModifierSO dmgMod in so.defensiveDamageModifiers)
        //{
        //    stats.defensiveModifiers.Add(dmgMod);
        //}


        //foreach (EnchantmentItem enchant in enchantItems)
        //{
        //    foreach (AttributeModifier mod in enchant.Modifiers)
        //    {
        //        stats.stats[mod.StatDefinition].AddModifier(mod);
        //    }
        //    EnchantmentItemSO enchantmentSO = enchant.SO<EnchantmentItemSO>();

        //    foreach (DamageModifierSO dmgMod in enchantmentSO.offensiveDamageModifiers)
        //    {
        //        stats.offensiveModifiers.Add(dmgMod);
        //    }
        //    foreach (DamageModifierSO dmgMod in enchantmentSO.defensiveDamageModifiers)
        //    {
        //        stats.defensiveModifiers.Add(dmgMod);
        //    }

        //}


    }

    public void OnUnequip(UnitStats stats)
    {
        EquipmentSO so = SO<EquipmentSO>();

        //foreach (AttributeModifier mod in baseModifiers)
        //{
        //    stats.stats[mod.StatDefinition].RemoveModifier(mod);
        //}
        //foreach (AttributeModifier mod in addedModifiers)
        //{
        //    stats.stats[mod.StatDefinition].RemoveModifier(mod);
        //}
        //foreach (DamageModifierSO dmgMod in so.offensiveDamageModifiers)
        //{
        //    stats.offensiveModifiers.Remove(dmgMod);
        //}
        //foreach (DamageModifierSO dmgMod in so.defensiveDamageModifiers)
        //{
        //    stats.defensiveModifiers.Remove(dmgMod);
        //}


        //foreach (EnchantmentItem enchant in enchantItems)
        //{
        //    foreach (AttributeModifier mod in enchant.Modifiers)
        //    {
        //        stats.stats[mod.StatDefinition].RemoveModifier(mod);
        //    }
        //    EnchantmentItemSO enchantmentSO = enchant.SO<EnchantmentItemSO>();

        //    foreach (DamageModifierSO dmgMod in enchantmentSO.offensiveDamageModifiers)
        //    {
        //        stats.offensiveModifiers.Remove(dmgMod);
        //    }
        //    foreach (DamageModifierSO dmgMod in enchantmentSO.defensiveDamageModifiers)
        //    {
        //        stats.defensiveModifiers.Remove(dmgMod);
        //    }

        
    }


    public override string GetFullDescription()
    {
        EquipmentSO so = SO<EquipmentSO>();
        string text = $"{so.Description}\n\n\n ";
        //foreach (AttributeModifier modifier in addedModifiers)
        //{
        //    text += modifier.ToString() + "\n";
        //}
        text += $"\nSlot: {so.Slot}\n\n";
        text += $"RequiredLevel: {so.RequiredLevel}\n\n";
        //text += $"Weight: {Weight}";
        return text;
    }

}


