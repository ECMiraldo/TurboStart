using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable]
public class PersistentData
{
    [field: JsonIgnore] public static event Action<long> onGoldChanged;

    private long _gold = 0;
    public long gold { 
        get { return _gold; }
        set { _gold = value;
            onGoldChanged?.Invoke(_gold);
        }
    }

    public long lastTickTime;
    public List<HeroData> heroes;
    public Inventory inventory;
    public TavernData tavernData;

}

