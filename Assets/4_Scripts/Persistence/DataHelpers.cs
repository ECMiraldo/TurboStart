using System;
using Persistence;
using System.Linq;

public static class DataHelpers
{
    public static float GetPartyStat(PartyStat stat)
    {
        return SaveLoadSystem.Instance.data.partyData.partyStats[stat].Value;
    }

    public static float GetSumOfStats(UnitStat stat)
    {
        return SaveLoadSystem.Instance.data.partyData.heroes.Sum(
            (x) => x.template.GetStats(x.level).TryGetValue(stat, out Attribute attr) ? attr.Value : 0
        );
    }
}