using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HeroData
{
    [JsonProperty][field: SerializeField] public string name { get; private set; }
    [JsonProperty][field: SerializeField] public int heroId { get; private set; }
    [JsonProperty][field: SerializeField] public string templateId { get; private set; }


    [JsonProperty][field: SerializeField] public int level { get; private set; }
    [JsonProperty][field: SerializeField] public HeroAction Action { get; protected set; }
    [JsonProperty][field: SerializeField] public string spriteName { get; private set; }
    [JsonProperty][field: SerializeField] public UnitStats attributes { get; private set; }
    [JsonProperty][field: SerializeField] public UnitVitals vitals { get; private set; }

    [JsonIgnore] private Sprite _sprite;
    [JsonIgnore]
    public Sprite Sprite
    {
        get
        {
            if (_sprite != null) return _sprite;
            else _sprite = Resources.Load<Sprite>($"{Constants.HERO_SPRITES_ADDRESS}/{spriteName}");
            return _sprite;
        }
    }

    public HeroData(int id, HeroTemplateSO heroTemplate, string name, string spriteAddress)
    {
        this.name = name;
        this.templateId = heroTemplate.id;
        this.heroId = id;
        this.spriteName = spriteAddress;
        this.level = 1;
        this.attributes = new UnitStats(heroTemplate.starterStats);
        this.vitals = new UnitVitals();
        vitals.InjectStats(attributes);
    }

    public HeroTemplateSO SO() => Database.heroTemplates[templateId];

    public void AssignAction(HeroAction action)
    {
        this.Action = action;
        action.OnActionFinished += OnActionFinished;
        
    }
    protected virtual void OnActionFinished(HeroAction finishedAction)
    {
        Action.OnActionFinished -= OnActionFinished;
        Action = null;
       
    }





    [JsonConstructor]
    public HeroData(int heroId, string templateId, int level, string name, string spriteName, HeroAction Action, UnitStats attributes, UnitVitals vitals)
    {
        this.name = name;
        this.templateId = templateId;
        this.heroId = heroId;
        this.spriteName = spriteName;
        this.Action = Action;
        this.level = level;
        this.vitals = vitals;
        if (attributes == null) this.attributes = new UnitStats(SO().starterStats);
        else this.attributes = attributes;
        vitals.InjectStats(attributes);
    }
}



