using AYellowpaper.SerializedCollections;

public interface IUnitData
{
    public SerializedDictionary<UnitStat, CharacterAttribute> GetStatsMap();
}


