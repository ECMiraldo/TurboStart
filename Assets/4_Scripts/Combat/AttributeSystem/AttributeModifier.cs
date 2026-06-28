
using System;
using UnityEngine;

public enum ModifierSource : byte
{
    Flat,
    skill,
    items,
    

}

[Serializable]
public class AttributeModifier 
{
    public event Action<float> OnChanged;

    [field: SerializeField] public UnitStat stat { get; private set; }
    [field: SerializeField] public float value { get; private set; }
    [field: SerializeField] public ModifierSource source { get; private set; }
    public AttributeModifier(UnitStat stat, float value, ModifierSource source)
    {
        this.stat = stat;
        this.value = value;
        this.source = source;
    }

    public void ChangeValue(float newValue)
    {
        value = newValue;
        OnChanged?.Invoke(newValue);
    }

    public void Apply(StatsComponent holder)
    {
        holder.stats[stat]?.AddModifier(this);
    }

    public void Remove(StatsComponent holder)
    {
        holder.stats[stat]?.RemoveModifier(this);
    }

    public override string ToString()
    {
        return $"{value}";
    }
}


