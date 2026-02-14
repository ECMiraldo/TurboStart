using NaughtyAttributes;
using System.Linq;
using UnityEngine;


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
    [field: SerializeField] public WeaponType WeaponType { get; private set; }
    [field: SerializeField] public Element Element { get; private set; }
    [field: SerializeField] public bool IsTwoHanded { get; private set; }
    public override Item ToItem() => new Weapon(this);

    public override bool CanEquip(CharacterData characterData)
    {
        return characterData.baseLevel >= RequiredLevel &&
                   Database.GetClass(characterData.classes.Last()).equippableWeapons.Contains(WeaponType);
    }

    public override string GetFullDescription()
    {
        string text = $"{Description}\n\n\n";

        if (IsTwoHanded) text += "TwoHanded\n";
        text += $"Damage type: {damageType} \n";
        text += $"DamageRange: {minAttack.min}/{minAttack.max} - {maxAttack.min}/{maxAttack.max} \n";

        foreach (AttributeModifier modifier in attributeModifiers)
        {
            text += modifier.ToString() + "\n";
        }
        foreach (DamageModifierSO modifier in offensiveDamageModifiers)
        {
            text += modifier.ToString() + "\n";
        }
        foreach (DamageModifierSO modifier in defensiveDamageModifiers)
        {
            text += modifier.ToString() + "\n";
        }


        text += $"Base ASPD: {AttackSpeed}\n";
        text += $"Range: {Range}\n";
        text += $"\nRequiredLevel: {RequiredLevel}\n\n";


        return text;
    }
}
