using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;


[Serializable]
public class PersistentData
{
    public long lastTickTime;
    [SerializeReference] public List<HeroData> heroes;
    public Inventory inventory;
    public TavernData tavernData;
    public ResourceData resourceData;
    public MapData mapData;
    public List<CityData> cityData;
}
