using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(menuName = "Combat/Stage")]
public class StageDefinitionSO : ScriptableObject
{
    public string stageId;
    public List<RoundDefinitionSO> rounds;
}
