using UnityEngine;
using System;
using System.Collections.Generic;
using NaughtyAttributes;

[Serializable]
public class StatGrowthRule
{
    public UnitStat stat;

    [Header("Scaling")]
    public float flatGainPerLevel;
    public float percentGainPerLevel;

    [Header("Rating")]
    [Min(0)]
    public float ratingWeight = 1f;
}

[CreateAssetMenu(menuName = "Combat/Unit Archetype")]
public class UnitArchetypeSO : IDScriptableObject
{
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