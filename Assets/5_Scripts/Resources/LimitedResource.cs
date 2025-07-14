using System;
using UnityEngine;

[Serializable]
public class LimitedResource
{
    public event Action<ulong> OnResourceChanged;

    private ulong value = 0;
    private Attribute maxValue;


    public ulong Value
    {
        get { return value; }
        set
        {
            this.value = value;
            OnResourceChanged?.Invoke(value);
        }
    }


    public ulong MaxValue => (ulong)Mathf.FloorToInt(maxValue.FinalValue);

    public void ApplyModifier(AttributeModifier mod)
    {
        maxValue.AddModifier(mod);
        OnResourceChanged.Invoke(value);
    }

    public void RemoveModifier(AttributeModifier mod)
    {
        maxValue.RemoveModifier(mod);
        OnResourceChanged.Invoke(value);
    }
}
