using NaughtyAttributes;
using Newtonsoft.Json;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


[Serializable]
public class HeroData
{
    //privates
    [SerializeField, JsonProperty] private string templateId;

    //publics
    public string name;
    public int level;
    public List<Equipment> equipments;

    [JsonIgnore] public HeroTemplateSO template => Database.heroTemplates[templateId];
    [JsonIgnore] public SerializedDictionary<UnitStat, CharacterAttribute> stats;

    [JsonConstructor] public HeroData() {}
    public HeroData(HeroTemplateSO template)
    {
        this.templateId = template.id;
        this.name = template.name;
        this.equipments = new List<Equipment>(Enum.GetValues(typeof(EquipmentSlot)).Length);
        for (int i = 0; i < equipments.Capacity; i++)
        {
            equipments.Add(null);
        }
    }

    [OnDeserialized] 
    private void OnDeserialized(StreamingContext context)
    {
        stats = template.GetStats(level);
    }

}
