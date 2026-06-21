using UnityEngine;


public class MapCityLocationButton : MapLocationButton
{
    [field: SerializeField] public CitySO cityDefinitionSO;
    [field: SerializeField] public CityScreenUI cityScreenUI;
    public override MapLocationSO mapLocationSO => cityDefinitionSO;

    public override void Enter()
    {
        cityScreenUI.SetCity(cityDefinitionSO);
        cityScreenUI.Open();

        //CombatSessionManager.Instance.StartStage(stageDefinitionSO);
    }
}
