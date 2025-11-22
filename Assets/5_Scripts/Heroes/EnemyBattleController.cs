using UnityEngine;

public class EnemyBattleController : Unit
{
    [field: SerializeField] public EnemyDataSO enemyData { get; private set; }

    public override UnitVitals unitVitals => vitals;
    [SerializeField] private UnitVitals vitals;
  
    public override UnitStats stats => enemyStats;
    [SerializeField] private UnitStats enemyStats;
   
    private void Awake()
    {
        enemyStats = new UnitStats(enemyData);
        vitals = new UnitVitals();
        vitals.InjectStats(enemyStats);
    }


}


