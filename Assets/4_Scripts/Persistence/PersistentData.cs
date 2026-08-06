using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;


[Serializable]
public class PersistentData
{
    public long lastTickTime;
    public PartyData partyData;
    public Inventory inventory;
    public TavernData tavernData;
    public ResourceData resourceData;
    public MapData mapData;
    public List<CityData> cityData; 
    public ProgressionData progressionData;
}
