using System;
using UnityEngine;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Newtonsoft.Json;

[Serializable]
public class PartyData
{
    public List<HeroData> heroes = new List<HeroData>();

    [JsonIgnore] public SerializedDictionary<PartyStat, Attribute> partyStats;

    [JsonConstructor] public PartyData() { }
    public PartyData(List<HeroData> heroes)
    {
        this.heroes = heroes;
        partyStats = new();
        foreach (PartyStat stat in Enum.GetValues(typeof(PartyStat)))
        {
            partyStats.Add(stat, new Attribute(1));
        }
    }

}