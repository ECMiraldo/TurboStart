using AYellowpaper.SerializedCollections;
using System.Collections;
using UnityEngine;


public abstract class BattleMoveController : MonoBehaviour
{
    [field: SerializeField] public Unit unit { get; private set; }
    [field: SerializeField] public UnitBattleMoveSO moveData { get; private set; }
    protected WaitForSeconds yieldTime;



    protected virtual void Awake()
    {
        yieldTime = new WaitForSeconds(moveData.moveTime);
    }

    public virtual IEnumerator DoMove()
    {
        unit.animator.SetTrigger(Constants.animationHashes[AnimationNames.Attack]);
        yield return yieldTime;
    }

}

public class BasicAttackMove : BattleMoveController
{

    public override IEnumerator DoMove()
    {

        yield return yieldTime;
    }
}



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


    [field: SerializeField] public AnimationNames animationName { get; private set; }
    [field: SerializeField] public BattleMoveTarget target { get; private set; }
    [field: SerializeField] public float moveTime { get; private set; }
 
}


