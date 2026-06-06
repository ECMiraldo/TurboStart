using NaughtyAttributes;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;

public enum WeaponType
{
    Staff = 0,
    Bow = 1,
    Sword = 2,
    
}



[CreateAssetMenu(menuName = "Items/Weapon")]
public class WeaponSO : EquipmentSO
{
    [field: Space(25)]
    [field: Header("Weapon")]
    //[field: SerializeField] public RollableInt minAttack { get; private set; }
    //[field: SerializeField] public RollableInt maxAttack { get; private set; }
    [field: SerializeField] public float AttackSpeed { get; private set; }
    [field: SerializeField, ReadOnly] public override EquipmentSlot Slot => EquipmentSlot.Weapon;
    [field: SerializeField] public WeaponType WeaponType { get; private set; }
    public override Item ToItem() => new Weapon(this);

    public override bool CanEquip(HeroData heroData)
    {
        HeroTemplateSO hero = heroData.template;
        return hero.usableWeapons.Contains(WeaponType);
    }

    public override string GetFullDescription()
    {
        string text = $"{Description}\n\n\n";

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

        //text += $"Damage type: {so.damageType} \n";
        text += $"DamageRange: {minAttackValue} - {maxAttackValue} \n";

        //foreach (AttributeModifier modifier in addedModifiers)
        //{
        //    text += modifier.ToString() + "\n";
        //}

        text += $"Base ASPD: {1 + so.AttackSpeed}\n";
        text += $"\nRequiredLevel: {so.RequiredLevel}\n\n";


        return text;
    }
}

