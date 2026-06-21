using System.Collections.Generic;
using Persistence;
using UnityEngine;

public class CityScreenUI : UIPanelController 
{
    [field: SerializeField] public CitySO currentCity { get; private set; }
    public void SetCity(CitySO citySO) => this.currentCity = citySO;

    private List<CityData> cityData;
    private void Awake()
    {
        cityData = SaveLoadSystem.Instance.data.cityData;
    }

    public override void Open()
    {
        base.Open();
    }
}
