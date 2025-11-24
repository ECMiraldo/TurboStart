using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/EnemyData")]
public class EnemyDataSO : ScriptableObject {
    [field: SerializeField] public GameObject prefab { get; private set; }


    [field: SerializeField] public FighterData fighterData { get; private set; }
}
