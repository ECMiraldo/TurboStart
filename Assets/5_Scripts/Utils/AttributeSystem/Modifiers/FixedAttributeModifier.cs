using Newtonsoft.Json;
using System;
using UnityEngine;

[Serializable]
public class FixedAttributeModifier : AttributeModifier
{
    [field: SerializeField] public float value { get; private set; }
    [field: SerializeField, ReadOnly] public override float Value => value;



    [JsonConstructor]
    public FixedAttributeModifier(float value, StatModType modType, string source) : base(modType, source, value)
    {
        this.value = value;
    }

    public FixedAttributeModifier(RollableAttributeModifier mod) : base(mod.ModType, mod.Source, mod.Value)
    {
        this.value = mod.Value;
    }

    public void ChangeValue(float val)
    {
        value = val;
        OnValueChanged?.Invoke();
    }
}
