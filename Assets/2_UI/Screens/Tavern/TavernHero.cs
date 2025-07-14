using Newtonsoft.Json;
using System;
using UnityEngine;

[Serializable]
public class TavernHero {
    [JsonProperty] [field:SerializeField] public int cost { get; private set; }
    [JsonProperty] [field: SerializeField] public Hero hero { get; private set; }

    [JsonConstructor]
    public TavernHero(int cost, Hero hero) 
    {
        this.cost = cost;
        this.hero = hero;
    }

    private static int GetNewHeroTicks()
    {
        return 200;
    }
    private static string GetRandomName()
    {
        return "random name";
    }

}
