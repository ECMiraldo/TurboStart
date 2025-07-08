using Newtonsoft.Json;
using System;
using UnityEngine;

[Serializable]
public class TavernHero : Hero
{
    [JsonProperty] [field:SerializeField] public int cost { get; private set; }


    [JsonConstructor]
    public TavernHero(int cost, int id, string name, string sprite, HeroAction action) : base(id, name, sprite, action)
    {
        this.cost = cost;
    }

    public TavernHero(int id, int nTicks, string sprite) : base(id, GetRandomName(), sprite)
    {
        AssignAction(new HeroAction(nTicks, HeroActionType.Tavern));
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
