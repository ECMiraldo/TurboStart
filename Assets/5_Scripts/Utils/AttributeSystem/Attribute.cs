using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;


public enum StatModType
{
    Flat = 0,
    PercentAdd = 1,
    PercentMult = 2,
}

[Serializable]
public class Attribute
{
    public event Action<float> OnValueChanged;

    [field: SerializeField] private float baseValue;
    [field: SerializeField, ReadOnly] public float FinalValue { get; private set; }

    [field: SerializeField] public List<AttributeModifier> Modifiers { get; private set; }

    public Attribute(float baseValue)
    {
        this.baseValue = baseValue;
        Modifiers = new();
        Recalculate();
    }

    public void SetBaseValue(float newValue)
    {
        baseValue = newValue;
        Recalculate();
    }

    public void AddModifier(AttributeModifier mod)
    {
        if (!Modifiers.Contains(mod))
        {
            Modifiers.Add(mod);
            mod.OnValueChanged += Recalculate;
            Recalculate();
        }
    }

    public void RemoveModifier(AttributeModifier mod)
    {
        if (Modifiers.Remove(mod))
        {
            Recalculate();
            mod.OnValueChanged -= Recalculate;
        }
    }

    public void ClearModifiersFromSource(string source)
    {
        if (Modifiers.RemoveAll(m => m.Source == source) > 0)
        {
            Recalculate();
        }
    }

    private void Recalculate()
    {
        float value = baseValue;
        float percentAdd = 0f;
        float percentMult = 1f;

        foreach (var mod in Modifiers)
        {
            switch (mod.ModType)
            {
                case StatModType.Flat:
                    
                    value += mod.Value;
                    break;
                case StatModType.PercentAdd:
                    percentAdd += mod.Value;
                    break;
                case StatModType.PercentMult:
                    percentMult *=  mod.Value;
                    break;
            }
        }
        float finalPercent = 1 + (percentAdd / 100.0f) * (1 + percentMult / 100.0f);
        value *= finalPercent;
        FinalValue = value;
        OnValueChanged?.Invoke(FinalValue);
    }

    public override string ToString()
    {
        return GetDisplayValue().ToString(); // Default to 0 decimal places for most attributes in display
    }

    public float GetDisplayValue(int decimalPlaces = 0) => (float)Math.Round(FinalValue, decimalPlaces);
}
