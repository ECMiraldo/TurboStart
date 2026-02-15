using Newtonsoft.Json;
using System;
using UnityEngine;

[Serializable]
public class TavernHero {
    [JsonProperty] [field:SerializeField] public int cost { get; private set; }
    [JsonProperty] [field: SerializeField] public HeroData hero { get; private set; }

    [JsonConstructor]
    public TavernHero(int cost, HeroData hero) 
    {
        this.cost = cost;
        this.hero = hero;
    }

  }
