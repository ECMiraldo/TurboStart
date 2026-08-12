using UnityEngine;
using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.VisualScripting;

[Serializable]
public class StatGrowthRule
{
    public UnitStat stat;

    [Header("Scaling")]
    public float flatGainPerLevel;
    public float percentGainPerLevel;
}

[CreateAssetMenu(menuName = "Combat/Unit Archetype")]
public class UnitArchetypeSO : IDScriptableObject
{
    public float focusPerHit;
    public float focusOnDamageTaken;
    public float focusPerSecond;

    [ReadOnly] public float offensiveRatingWeight;
    [ReadOnly] public float magicRatingWeight;
    [ReadOnly] public float defensiveRatingWeight;

    public List<StatGrowthRule> statGrowthRules = new();

    
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (statGrowthRules.Count != 0)
            return;
            
        foreach (UnitStat stat in Enum.GetValues(typeof(UnitStat)))
        {
            if (!statGrowthRules.Exists(r => r.stat == stat))
                statGrowthRules.Add(new StatGrowthRule { stat = stat });
        }
    }
#endif

    public StatGrowthRule GetRule(UnitStat stat)
    {
        return statGrowthRules.Find(r => r.stat == stat);
    }


    
}