using System;
using System.Collections.Generic;
using Newtonsoft.Json;


[Serializable]
public class PersistentData
{
    public long lastTickTime;
    public List<HeroData> heroes;
    public Inventory inventory;
    public TavernData tavernData;
    public ResourceData resourceData;
    public MapData mapData;
}

public class MapData
{
    public int currentLocationIndex;
    public int destinationLocationIndex;
    public int remainingTravelTicks;
}
