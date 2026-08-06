using System.Collections.Generic;
using Persistence;
using Unity.Collections;
using UnityEngine;


public class CityScreenUI : UIPanelController 
{
    [field: SerializeField] public CitySO currentCity { get; private set; }
    public void SetCity(CitySO citySO) => this.currentCity = citySO;

    private List<CityData> cityData;
    [field: SerializeField] public CityData currentCityData {get; private set;}
    protected override void Awake()
    {
        cityData = SaveLoadSystem.Instance.data.cityData;
    }

    public override void Open()
    {
        base.Open();
        SyncCityData();
    }

    private void SyncCityData()
    {
        var data = cityData.Find(x => x.id == currentCity.id);
        if (data == null)
        {
            data = new CityData();
            cityData.Add(data);
        } 
        currentCityData = data;
    }

}
