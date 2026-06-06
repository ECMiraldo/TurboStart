using NaughtyAttributes;
using Newtonsoft.Json;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;

[Serializable]
public class HeroStatData
{
    [field: JsonProperty, SerializeField, ReadOnly]
    private SerializedDictionary<UnitStat, CharacterAttribute> stats;

    public HeroStatData(HeroTemplateSO template)
    {
        stats = template.GetStats();
    }

    public SerializedDictionary<UnitStat, CharacterAttribute> GetStatCopy()
    {
        var copy = new SerializedDictionary<UnitStat, CharacterAttribute>();

        foreach (var kvp in stats)
        {
            copy[kvp.Key] = new CharacterAttribute(kvp.Value.Value);
        }

        return copy;
    }

    [JsonConstructor]
    public HeroStatData(SerializedDictionary<UnitStat, CharacterAttribute>  stats)
    {
        this.stats = stats;
    }

}

[Serializable]
public class HeroData
{
    [JsonProperty, SerializeField] public string templateId { get; private set; }
    [JsonProperty, SerializeField] public string name { get; private set; }
    [JsonProperty, SerializeField] public HeroStatData statData { get; private set; }
    [JsonProperty, SerializeField] public HeroEquipmentData equipmentData { get; private set; }
    [JsonIgnore] public HeroTemplateSO template => Database.heroTemplates[templateId];

    public HeroData(HeroTemplateSO template)
    {
        this.templateId = template.id;
        this.statData = new(template);
        this.equipmentData = new HeroEquipmentData();


    }

    [JsonConstructor]
    public HeroData(string templateId, HeroStatData statData, HeroEquipmentData equipmentData)
    {
        this.templateId = templateId;
        this.statData = statData;
        this.equipmentData = equipmentData;
    }


}

[Serializable]
public class HeroEquipmentData
{
    [JsonProperty, SerializeField] public List<Equipment> equipments;

    public HeroEquipmentData()
    {
        equipments = new List<Equipment>(Enum.GetValues(typeof(EquipmentSlot)).Length);
        for (int i = 0; i < equipments.Capacity; i++)
        {
            equipments.Add(null);
        }
    }
}
