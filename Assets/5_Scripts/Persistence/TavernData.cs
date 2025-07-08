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

    public TavernHero CreateHero(string sprite)
    {
        TavernHero newHero = new TavernHero(totalCreatedHeroes, GetNewHeroTicks(), sprite );
        availableHeroes.Add(newHero);
        totalCreatedHeroes++;
        return newHero;
    }
    private int GetNewHeroTicks()
    {
        return 200;
    }

}
