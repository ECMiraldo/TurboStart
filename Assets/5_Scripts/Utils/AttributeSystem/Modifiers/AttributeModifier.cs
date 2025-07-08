using Newtonsoft.Json;
using System;
using UnityEngine;

[Serializable]
public abstract class AttributeModifier
{
    [JsonIgnore] public Action OnValueChanged;

    [field: SerializeField] public StatModType ModType { get; protected set; }
    [field: SerializeField] public string Source { get; protected set; }
    [field: SerializeField] public virtual float Value { get; }

    public AttributeModifier() { }

    [JsonConstructor]
    public AttributeModifier(StatModType modType, string source, float value) 
    {
        this.ModType = modType;
        this.Source = source;
        this.Value = value;
    }

    public float GetDisplayValue(int decimalPlaces)
    {
        return (float)Math.Round(Value, decimalPlaces);
    }

 }
