using System;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class ResourceData
{
    [field: JsonIgnore] public static event Action<long> onGoldChanged;
    [field: JsonIgnore] public static event Action<long> onExperienceChanged;
    private long _gold = 0;
    private long _experience = 0;
    public long gold
    {
        get { return _gold; }
        private set
        {
            _gold = value;
            onGoldChanged?.Invoke(_gold);
        }
    }
    
    public long experience
    {
        get { return _experience; }
        private set
        {
            _experience = value;
            onExperienceChanged?.Invoke(_experience);
        }
    }

    public void AddGold(int amount, bool withModifiers = true) 
    {
        if (withModifiers) amount = Mathf.FloorToInt(amount * DataHelpers.GetSumOfStats(UnitStat.GoldDrop));
        gold += amount;
    }

    public void AddExperience(int amount, bool withModifiers = true)
    {
        if (withModifiers) amount = Mathf.FloorToInt(amount * DataHelpers.GetSumOfStats(UnitStat.ExperienceGain));
        experience += amount;
    }

}

