using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class CityData
{
    public string id;

    [Header("State")]
    public long lastSavedTick = 0;


    [Header("Shop")]
    [SerializeField] public List<ShopEntry> shopItems;



}