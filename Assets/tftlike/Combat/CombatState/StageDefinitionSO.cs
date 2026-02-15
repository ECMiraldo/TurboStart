using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(menuName = "Combat/Stage")]
public class StageDefinitionSO : IDScriptableObject
{
    public int requiredLevel;
    public string stageName;
    public List<RoundDefinitionSO> rounds;
}
