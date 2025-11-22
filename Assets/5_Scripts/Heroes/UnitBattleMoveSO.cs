using UnityEngine;

public enum BattleMoveTarget : byte
{
    self = 0,
    teamFront = 1,
    teamBack = 2,
    wholeTeam = 3,
    EnemyFront = 4,
    EnemyBack = 5,
    EnemyTeam = 6,
}





[CreateAssetMenu(menuName = "Combat/Battle Move")]
public class UnitBattleMoveSO : ScriptableObject
{
    [field: SerializeField] public string moveName { get; private set; }
    [field: SerializeField] public Sprite moveIcon { get; private set; }

    [TextArea]
    [field: SerializeField] public string moveDescription { get; private set; }
    [field: SerializeField] public GameObject moveController { get; private set; }


    [field: SerializeField] public BattleMoveTarget target { get; private set; }
    [field: SerializeField] public float moveTime { get; private set; }
 
}


