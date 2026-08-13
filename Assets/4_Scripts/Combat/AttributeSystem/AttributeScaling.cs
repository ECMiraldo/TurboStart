using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class AttributeScaling
{
    public UnitStat stat;
    public float scalingFactor;

    //unused
    public static float SumOfScalings(IEnumerable<AttributeScaling> scalings, StatsComponent stats)
    {
        float sum = 0;
        foreach (var scaling in scalings)
        {
            if (stats.stats.TryGetValue(scaling.stat, out var attribute))
            {
                sum += attribute.Value * scaling.scalingFactor;
            }
        }
        return sum;
    }
}