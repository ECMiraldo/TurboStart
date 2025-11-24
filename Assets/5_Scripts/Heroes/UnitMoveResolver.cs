using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMoveResolver :  MonoBehaviour
{
    [field: SerializeField] public List<UnitBattleMoveSO> orderedMoves = new (4);

    private List<BattleMoveController> controllers = new();
    private int currentMoveIndex = 0;

    private void Start()
    {
        InstantiateMoveControllers(); 
    }

    protected virtual void InstantiateMoveControllers()
    {
        GameObject movesParent = new GameObject("BattleMoveControllers");
        movesParent.transform.parent = transform;
        
        foreach (var battleMoveData in orderedMoves)
        {
            BattleMoveController newController = Instantiate(battleMoveData.moveController, movesParent.transform).GetComponent<BattleMoveController>();
            controllers.Add(newController);
        }
    }

    public IEnumerator DoMove()
    {
        if (currentMoveIndex == controllers.Count) currentMoveIndex = 0;

        yield return controllers[currentMoveIndex].DoMove();
        currentMoveIndex++;
    }

}


