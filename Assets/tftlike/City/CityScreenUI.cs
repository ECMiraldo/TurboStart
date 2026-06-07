using UnityEngine;

public class CityScreenUI : UIPanelController 
{
    [field: SerializeField] public CitySO currentCity { get; private set; }
    public void SetCity(CitySO citySO) => this.currentCity = citySO;
}
