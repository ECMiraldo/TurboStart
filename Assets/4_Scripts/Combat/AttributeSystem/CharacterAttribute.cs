using AYellowpaper.SerializedCollections;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;


[Serializable]
public class Attribute
{

    public event Action<float> OnValueChanged;

    [field: SerializeField][JsonProperty] protected float starterValue;
    [field: SerializeField][JsonProperty] protected float value;

    [JsonIgnore] public float Value => isDirty ? value = Recalculate() : value;


    [field: SerializeField] protected bool isDirty;
    [field: SerializeField] public SerializedDictionary<ModifierSource, List<AttributeModifier>> Modifiers { get; private set; }

    public Attribute(float starterValue)
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
            if(source == ModifierSource.Flat)
            {
                totalSum += Modifiers[source].Sum(x => x.value);
                continue;
            }

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

    public string ToString(int ndecimals)
    {
        if (ndecimals <= 0)
            return Mathf.RoundToInt(Value).ToString();

        string format = "0." + new string('#', ndecimals);
        return Value.ToString(format);
    }

}


