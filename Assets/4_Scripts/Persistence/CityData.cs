using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class CityData
{
    public string id;

    [Header("State")]
    public int lastVisitedDay = 0;


    [Header("Shop")]
    [SerializeField] public List<ShopEntry> shopItems;



}