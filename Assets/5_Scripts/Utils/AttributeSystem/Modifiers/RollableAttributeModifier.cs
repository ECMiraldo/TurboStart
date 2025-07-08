using Newtonsoft.Json;
using System;
using UnityEngine;

[Serializable]
public class RollableAttributeModifier : AttributeModifier
{
    [field: SerializeField] public RollableFloat RollRange { get; private set; }
    [field: SerializeField, ReadOnly] public override float Value => RollRange.Value;


    [JsonConstructor]
    public RollableAttributeModifier(RollableFloat range, StatModType modType, string source) : base(modType, source, range.Value)
    {
        this.RollRange = range;
    }

    public RollableAttributeModifier(RollableAttributeModifier mod) : base(mod.ModType, mod.Source, mod.Value)
    {
        this.RollRange = mod.RollRange;
    }
} 
