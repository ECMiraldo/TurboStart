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
    public List<TavernHero> availableHeroes;
    [JsonProperty] private int totalCreatedHeroes;

    public TavernData()
    {
        availableHeroes = new();
    }

    [JsonConstructor]
    public TavernData(List<TavernHero> availableHeroes)
    {
        this.availableHeroes = availableHeroes;
    }


    public void AddHero()
    {
        TavernHero newHero = new TavernHero(GetNewHeroTicks(), HeroFactory.CreateRandomHero(totalCreatedHeroes));
        availableHeroes.Add(newHero);
        newHero.hero.AssignAction(new HeroAction(GetNewHeroTicks(), HeroActionType.Tavern));
        //check when action finishes then remove hero
        totalCreatedHeroes++;
    }

    private int GetNewHeroTicks()
    {
        return 200;
    }


}
