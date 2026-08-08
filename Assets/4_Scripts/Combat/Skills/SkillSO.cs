using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Skill")]
public class SkillSO : IDScriptableObject
{
    [field: SerializeField] public Sprite icon { get; private set; }
    [field: SerializeField] public string skillName { get; private set; }
    [field: SerializeField] public int minFocusCost { get; private set; }
    [field: SerializeField] public int baseEffectValue { get; private set; }
    [field: SerializeField] public float bonusPerFocus { get; private set; }
    [field: SerializeField] public DamageType damageType { get; private set; }

    [field: SerializeReference] public List<CombatEffect> onCastEffects = new();
    [field: SerializeReference] public List<CombatEffect> onDamageEffects = new();
    [field: SerializeReference] public List<AttributeScaling> statScales = new();

    public IEnumerator Cast(UnitContext ctx)
    {
        foreach (CombatEffect effect in onCastEffects)
        {
            yield return effect.Execute(ctx);
        }
    }

    public int GetEffectValue(StatsComponent stat, int focus)
    {
        float value = baseEffectValue + (bonusPerFocus * focus);

        foreach (AttributeScaling scaling in statScales)
        {
            if (stat.stats.TryGetValue(scaling.stat, out var attribute))
            {
                value *= attribute.Value * ( 1 + scaling.scalingFactor);
            }
        }

        return Mathf.FloorToInt(value);
    }
}
