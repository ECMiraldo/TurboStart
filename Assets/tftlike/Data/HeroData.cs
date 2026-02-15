using Newtonsoft.Json;
using UnityEngine;

public class HeroData
{
    [JsonProperty, SerializeField] public string templateId { get; private set; }
    [JsonProperty, SerializeField] public string name { get; private set; }

    
    [JsonIgnore] public HeroTemplateSO template => Database.heroTemplates[templateId];
    public HeroData(HeroTemplateSO template)
    {
        this.templateId = template.id;
    }

    [JsonConstructor]
    public HeroData(string templateId)
    {
        this.templateId = templateId;
    }


}
