using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class UnitVitals
{
    public event Action<int> OnHealthChanged;
    public event Action<int> OnManaChanged;
    public event Action OnDied;

    [field: SerializeField] public int currentHealth { get; private set; }
    [field: SerializeField] public int currentMana { get; private set; }

    [JsonIgnore] private UnitStats unitStats;
    public void InjectStats(UnitStats stats)
    {
        this.unitStats = stats;
        //handles regen in case the vitals are being created and not loaded.
        if (currentHealth == 0 && currentMana == 0)
        {
            IncrementHealth(unitStats.stats[UnitStat.Health].ToInt());
            IncrementMana(unitStats.stats[UnitStat.Mana].ToInt());
        }
    }
    public void IncrementHealth(int health)
    {
        currentHealth = Mathf.Clamp(currentHealth + health, 0, unitStats.stats[UnitStat.Health].ToInt());
        OnHealthChanged?.Invoke(currentHealth);
        if (currentHealth == 0)
        {
            //TODO: handle die
            OnDied?.Invoke();
        }
    }

    public void IncrementMana(int mana)
    {
        currentMana = Mathf.Clamp(currentMana + mana, 0, unitStats.stats[UnitStat.Mana].ToInt());
        OnManaChanged?.Invoke(currentMana);
    }


}



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
        this.attributes = new UnitStats(heroTemplate);
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
    public HeroData(int heroId, string templateId, int level, string name, string spriteAddress, HeroAction action, UnitStats attributes)
    {
        this.name = name;
        this.templateId = templateId;
        this.heroId = heroId;
        this.spriteName = spriteAddress;
        this.Action = action;
        this.level = level;
        this.attributes = attributes;
        vitals.InjectStats(attributes);
    }
}



