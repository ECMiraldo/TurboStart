using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "HeroTemplate")]
public class HeroTemplateSO : UnitDataSO
{
    [field: SerializeField] public List<ArmorType> usableArmors { get; private set; }
    [field: SerializeField] public List<WeaponType> usableWeapons { get; private set; }
    [field: SerializeField] public float expGrowPerLevel { get; private set; }
    [field: SerializeField] public float lvl1Exp { get; private set; }

    public int GetRequiredExpForLevel(int level)
    {
        if (level <= 1)
            return Mathf.CeilToInt(lvl1Exp);

        float requiredExp = lvl1Exp;
        for (int currentLevel = 2; currentLevel <= level; currentLevel++)
        {
            requiredExp += currentLevel * expGrowPerLevel;
        }

        return Mathf.CeilToInt(requiredExp);
    }
}
