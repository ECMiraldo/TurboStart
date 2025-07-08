using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityUtils;



[Serializable]
public class Hero
{
    [JsonProperty][field: SerializeField] public string Name { get; private set; }
    [JsonProperty][field: SerializeField] public int Id { get; private set; }
    [JsonProperty][field: SerializeField] public HeroAction Action { get; protected set; }
    [JsonProperty][field: SerializeField] public string spriteName { get; private set; }


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


    [JsonConstructor]
    public Hero(int id, string name, string spriteAddress, HeroAction action)
    {
        this.Name = name;
        this.Id = id;
        this.spriteName = spriteAddress;
        this.Action = action;
    }


    public Hero(int id, string name, string spriteAddress)
    {
        this.Name = name;
        this.Id = id;
        this.spriteName = spriteAddress; 
    }

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
}


