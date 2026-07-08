using System;
using Persistence;
using System.Linq;

public static class DataHelpers
{
    public static float GetSumOfStats(UnitStat stat)
    {
        return SaveLoadSystem.Instance.data.heroes.Sum(
            (x) => x.template.GetStats(x.level).TryGetValue(stat, out CharacterAttribute attr) ? attr.Value : 0
        );
    }
}