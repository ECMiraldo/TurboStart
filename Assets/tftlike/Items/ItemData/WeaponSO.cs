using NaughtyAttributes;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;



[CreateAssetMenu(menuName = "Items/Weapon")]
public class WeaponSO : EquipmentSO
{
    [field: Space(25)]
    [field: Header("Weapon")]
    [field: SerializeField] public RuntimeAnimatorController animationController { get; private set; }
    //[field: SerializeField] public DamageType damageType { get; private set; }
    //[field: SerializeField] public RollableInt minAttack { get; private set; }
    //[field: SerializeField] public RollableInt maxAttack { get; private set; }
    [field: SerializeField] public float Range { get; private set; }
    [field: SerializeField] public float AttackSpeed { get; private set; }
    [field: SerializeField, ReadOnly] public override EquipmentSlot Slot => EquipmentSlot.MainHand;
    //[field: SerializeField] public WeaponType WeaponType { get; private set; }
    //[field: SerializeField] public Element Element { get; private set; }
    [field: SerializeField] public bool IsTwoHanded { get; private set; }
    public override Item ToItem() => new Weapon(this);

    public override bool CanEquip(HeroData heroData)
    {
        return true;
    }

    public override string GetFullDescription()
    {
        string text = $"{Description}\n\n\n";

        if (IsTwoHanded) text += "TwoHanded\n";
        //text += $"Damage type: {damageType} \n";
        //text += $"DamageRange: {minAttack.min}/{minAttack.max} - {maxAttack.min}/{maxAttack.max} \n";

        //foreach (AttributeModifier modifier in attributeModifiers)
        //{
        //    text += modifier.ToString() + "\n";
        //}
        //foreach (DamageModifierSO modifier in offensiveDamageModifiers)
        //{
        //    text += modifier.ToString() + "\n";
        //}
        //foreach (DamageModifierSO modifier in defensiveDamageModifiers)
        //{
        //    text += modifier.ToString() + "\n";
        //}


        //text += $"Base ASPD: {AttackSpeed}\n";
        //text += $"Range: {Range}\n";
        //text += $"\nRequiredLevel: {RequiredLevel}\n\n";


        return text;
    }
}

public class Weapon : Equipment
{
    public int minAttackValue { get; private set; }
    public int maxAttackValue { get; private set; }

    //[field: SerializeField] public FixedAttributeModifier upgradeModifierMax { get; protected set; }

    public Weapon(WeaponSO weaponSO) : base(weaponSO)
    {
        //minAttackValue = Random.Range(weaponSO.minAttack.min, weaponSO.minAttack.max + 1);
        //maxAttackValue = Random.Range(weaponSO.maxAttack.min, weaponSO.maxAttack.max + 1);

        //if (weaponSO.damageType == DamageType.Physical)
        //{
        //    baseModifiers.Add(new FixedAttributeModifier(minAttackValue, AttributeEnum.MIN_PHYSICAL_DMG, StatModType.Flat, weaponSO.Name));
        //    baseModifiers.Add(new FixedAttributeModifier(maxAttackValue, AttributeEnum.MAX_PHYSICAL_DMG, StatModType.Flat, weaponSO.Name));
        //    upgradeModifier = new FixedAttributeModifier(0.0f, AttributeEnum.MIN_PHYSICAL_DMG, StatModType.PercentAdd, "Weapon Upgrade");
        //    upgradeModifierMax = new FixedAttributeModifier(0.0f, AttributeEnum.MAX_PHYSICAL_DMG, StatModType.PercentAdd, "Weapon Upgrade");

        //}
        //if (weaponSO.damageType == DamageType.Ranged)
        //{
        //    baseModifiers.Add(new FixedAttributeModifier(minAttackValue, AttributeEnum.MIN_RANGED_DMG, StatModType.Flat, weaponSO.Name));
        //    baseModifiers.Add(new FixedAttributeModifier(maxAttackValue, AttributeEnum.MAX_RANGED_DMG, StatModType.Flat, weaponSO.Name));
        //    upgradeModifier = new FixedAttributeModifier(0.0f, AttributeEnum.MIN_MAGICAL_DMG, StatModType.PercentAdd, "Weapon Upgrade");
        //    upgradeModifierMax = new FixedAttributeModifier(0.0f, AttributeEnum.MAX_MAGICAL_DMG, StatModType.PercentAdd, "Weapon Upgrade");
        //}
        //if (weaponSO.damageType == DamageType.Magical)
        //{
        //    baseModifiers.Add(new FixedAttributeModifier(minAttackValue, AttributeEnum.MIN_MAGICAL_DMG, StatModType.Flat, weaponSO.Name));
        //    baseModifiers.Add(new FixedAttributeModifier(maxAttackValue, AttributeEnum.MAX_MAGICAL_DMG, StatModType.Flat, weaponSO.Name));
        //    upgradeModifier = new FixedAttributeModifier(0.0f, AttributeEnum.MIN_MAGICAL_DMG, StatModType.PercentAdd, "Weapon Upgrade");
        //    upgradeModifierMax = new FixedAttributeModifier(0.0f, AttributeEnum.MAX_MAGICAL_DMG, StatModType.PercentAdd, "Weapon Upgrade");
        //}

        //baseModifiers.Add(new FixedAttributeModifier(weaponSO.AttackSpeed, AttributeEnum.ATTACK_SPEED, StatModType.PercentMult, weaponSO.Name));
        //baseModifiers.Add(new FixedAttributeModifier(weaponSO.Range, AttributeEnum.RANGE, StatModType.Flat, weaponSO.Name));
        //baseModifiers.Add(upgradeModifierMax);
    }

    //[JsonConstructor]
    //public Weapon()
    //{

    //}

    public override void Upgrade()
    {
        base.Upgrade();
        //upgradeModifierMax.ChangeValue(upgradeLevel + Mathf.Floor(upgradeLevel / 2));
    }

    public override string GetFullDescription()
    {
        WeaponSO so = SO<WeaponSO>();
        string text = $"{so.Description}\n\n\n";

        if (so.IsTwoHanded) text += "TwoHanded\n";
        //text += $"Damage type: {so.damageType} \n";
        text += $"DamageRange: {minAttackValue} - {maxAttackValue} \n";

        //foreach (AttributeModifier modifier in addedModifiers)
        //{
        //    text += modifier.ToString() + "\n";
        //}

        text += $"Base ASPD: {1 + so.AttackSpeed}\n";
        text += $"Range: {so.Range}\n";
        text += $"\nRequiredLevel: {so.RequiredLevel}\n\n";


        return text;
    }
}

