using UnityEngine;
using System;
using NaughtyAttributes;
using Newtonsoft.Json;

[Serializable]
public class RollableFloat {
    [field: SerializeField] public float min { get; private set; }
    [field: SerializeField] public float max { get; private set; }
    [field: ReadOnly] private float value;
    public float Value => GetValue();

    public RollableFloat() { }

    [JsonConstructor]
    public RollableFloat(float min, float max)
    {
        this.min = min;
        this.max = max;
    }


    private float GetValue()
    {
        if (value == 0)value = UnityEngine.Random.Range(min, max);
        return value;
    }
}
