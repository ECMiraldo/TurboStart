using UnityEngine;


public class MapCityLocationButton : MapLocationButton
{
    [field: SerializeField] public CitySO cityDefinitionSO;
    [field: SerializeField] public CityScreenUI cityScreenUI;
    public override MapLocationSO mapLocationSO => cityDefinitionSO;

    public override void Enter()
    {
        cityScreenUI.Open();
        cityScreenUI.SetCity(cityDefinitionSO);

        //CombatSessionManager.Instance.StartStage(stageDefinitionSO);
    }
}
