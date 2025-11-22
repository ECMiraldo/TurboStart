using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttribute
{

    public event Action<float> OnValueChanged;

    [field: SerializeField] protected float starterValue;
    [field: SerializeField] protected float value;

    public float Value => isDirty ? value = Recalculate() : value;


    [field: SerializeField] protected bool isDirty;
    [field: SerializeField] public SerializedDictionary<ModifierSource, List<AttributeModifier>> Modifiers { get; private set; }

    public CharacterAttribute(float starterValue)
    {
        value = starterValue;
        this.starterValue = starterValue;
        Modifiers = new();
        for (int i = 0; i < Enum.GetValues(typeof(ModifierSource)).Length; i++)
        {
            Modifiers.Add((ModifierSource)i, new());
        }
    }


    public void AddModifier(AttributeModifier mod)
    {
        if (!Modifiers[mod.source].Contains(mod))
        {
            Modifiers[mod.source].Add(mod);
            value = Recalculate();
            OnValueChanged?.Invoke(Value);
            mod.OnChanged += OnModifierChanged;
        }
    }

    public void RemoveModifier(AttributeModifier mod)
    {
        if (Modifiers[mod.source].Remove(mod))
        {
            value = Recalculate();
            mod.OnChanged -= OnModifierChanged;
            OnValueChanged?.Invoke(Value);
        }
    }

    private void OnModifierChanged(float value) => isDirty = true;

    public float Recalculate()
    {
        float totalSum = starterValue;
        foreach (var source in Modifiers.Keys)
        {
            float sourceSum = 1;
            foreach (var mod in Modifiers[source])
            {
                sourceSum += mod.value;
            }
            totalSum *= sourceSum;
        }
        return totalSum;
    }

    public int ToInt() => Mathf.FloorToInt(Value);

}


