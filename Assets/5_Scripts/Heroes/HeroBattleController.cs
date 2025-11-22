using UnityEngine;


public class HeroBattleController : Unit
{
    [field: SerializeField] public HeroData heroData { get; private set; }
    [field: SerializeField] public override UnitStats stats => heroData.attributes;
    [field: SerializeField] public override UnitVitals unitVitals => heroData.vitals;

    public void SetData(HeroData hero)
    {
        this.heroData = hero;
    }
}


