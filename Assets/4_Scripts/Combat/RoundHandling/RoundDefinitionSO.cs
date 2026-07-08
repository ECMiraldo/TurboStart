using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Round")]
public class RoundDefinitionSO : ScriptableObject
{
    [Header("Allowed Monsters")]
    public List<EnemyDataSO> monsterPool;

    [Header("Special Flags")]
    public bool isBossRound;
    public bool isEndurance;
}
