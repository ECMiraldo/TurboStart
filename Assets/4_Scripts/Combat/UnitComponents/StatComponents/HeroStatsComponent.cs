using System;
using System.Collections.Generic;
using UnityEngine;

public class HeroStatsComponent : StatsComponent
{
    [field: SerializeReference] public HeroData heroData { get; private set; }
    [field: SerializeField] public int stageLevel { get; private set; } = 1;
    [field: SerializeField] public int experience { get; private set; } = 0;
    private Dictionary<UnitStat, List<AttributeModifier>> levelModifiers = new();

    public void SetHeroData(HeroData data)
    {
        this.heroData = data;
    }
    public override void Init(UnitContext ctx)
    {
        base.Init(ctx);
        team = Team.Player;
        stats = heroData.template.GetStats(heroData.level);
        AddLevelModifiers();

        foreach(Equipment equipment in heroData.equipments)
        {
            if (equipment == null) continue;
            foreach (ItemEffect effect in equipment.effects)
            {
                effect.OnEnable(ctx);
            }
        }
    }

    private void AddLevelModifiers()
    {
        levelModifiers.Clear();

        foreach (StatGrowthRule growthRule in heroData.template.statGrowthRules)
        {
            if (!stats.ContainsKey(growthRule.stat))
                continue;

            var modifiers = new List<AttributeModifier>
            {
                new(growthRule.stat, 0f, ModifierSource.Flat),
                new(growthRule.stat, 0f, ModifierSource.StageLevel),
            };

            foreach (AttributeModifier modifier in modifiers)
            {
                modifier.Apply(this);
            }

            levelModifiers[growthRule.stat] = modifiers;
        }

        ApplyLevelScaling();
    }

    public void LevelUp()
    {
        stageLevel++;
        ApplyLevelScaling();
    }
    private void ApplyLevelScaling()
    {
        foreach (KeyValuePair<UnitStat, List<AttributeModifier>> pair in levelModifiers)
        {
            StatGrowthRule growthRule = heroData.template.statGrowthRules.Find(x => x.stat == pair.Key);
            if (growthRule == null || pair.Value.Count < 2)
                continue;

            pair.Value[0].ChangeValue(growthRule.flatGainPerLevel * stageLevel);
            pair.Value[1].ChangeValue(growthRule.percentGainPerLevel * stageLevel);
        }
    }




 

}
