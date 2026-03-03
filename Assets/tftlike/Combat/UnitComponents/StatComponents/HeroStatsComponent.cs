using System;
using UnityEngine;

public class HeroStatsComponent : StatsComponent
{
    [field: SerializeReference] public HeroData heroData { get; private set; }
    public override void Init(UnitContext ctx)
    {
        team = Team.Player;
    }

    public void SetHeroData(HeroData data)
    {
        this.heroData = data;

    }

}
