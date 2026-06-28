using System;
using UnityEngine;

public class HeroStatsComponent : StatsComponent
{
    [field: SerializeReference] public HeroData heroData { get; private set; }
    public void SetHeroData(HeroData data)
    {
        this.heroData = data;
    }
    public override void Init(UnitContext ctx)
    {
        team = Team.Player;
        stats = heroData.statData.GetStatCopy();

        foreach(Equipment equipment in heroData.equipmentData.equipments)
        {
            if (equipment == null) continue;
            foreach (ItemEffect effect in equipment.effects)
            {
                effect.OnEnable(ctx);
            }
        }
    }

 

}
