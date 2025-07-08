using UnityEngine;
using System;
using NaughtyAttributes;

[Serializable]
public class RollableInt
{
    [field: SerializeField] public int min { get; private set; }
    [field: SerializeField] public int max { get; private set; }

    [ReadOnly] private int value = 0;
    public int Value => GetValue(); 

    private int GetValue()
    {
        if (value == 0) value = UnityEngine.Random.Range(min, max);
        return value;
    }
}
