using System;
using Persistence;
using System.Linq;

public static class DataHelpers
{
    public static float GetPartyStat(PartyStat stat)
    {
        return SaveLoadSystem.Instance.data.partyData.partyStats[stat].Value;
    }
}