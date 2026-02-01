using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Round")]
public class RoundDefinitionSO : ScriptableObject
{
    public int roundIndex;

    [Header("Difficulty")]
    public float difficultyMultiplier = 1f;

    [Header("Spawn Budget")]
    public int totalPowerBudget = 10;

    [Header("Allowed Monsters")]
    public List<EnemyDataSO> monsterPool;

    [Header("Special Flags")]
    public bool isBossRound;
    public bool isEndurance;
}
