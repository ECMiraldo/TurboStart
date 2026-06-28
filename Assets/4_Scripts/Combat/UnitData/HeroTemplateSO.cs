using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "HeroTemplate")]
public class HeroTemplateSO : UnitDataSO
{
    [field: SerializeField] public List<ArmorType> usableArmors { get; private set; }
    [field: SerializeField] public List<WeaponType> usableWeapons { get; private set; }

}
