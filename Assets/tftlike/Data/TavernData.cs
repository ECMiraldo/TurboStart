using AYellowpaper.SerializedCollections;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityUtils;



[Serializable]
public class TavernData
{
    public List<(HeroData, long)> availableHeroes = new();
    public int nCardsShown = 3;
    public double durationInHours = 6;
    public long nextRefresh;
        
    public (HeroData,long) CreateHero()
    {
        HeroTemplateSO template = Utils.GetRandomFromList(Database.heroTemplates.Values.ToList());
        HeroData data = new HeroData(template);
        long cost = GetHeroCost();
        availableHeroes.Add((data,cost));
        return (data, cost);
    }

    private long GetHeroCost()
    {
        return 100;
    }

    private int GetNewHeroTicks()
    {
        return 200;
    }


}
