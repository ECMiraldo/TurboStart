using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/EnemyData")]
public class EnemyDataSO : ScriptableObject, IUnitData
{
    [field: SerializeField] public GameObject prefab { get; private set; }


    [field: SerializeField] public int maxHealth { get; set; }
    [field: SerializeField] public int maxMana { get; set; }
    [field: SerializeField] public int attackDamage { get; set; }
    [field: SerializeField] public int initiative { get; set; }



    public SerializedDictionary<UnitStat, CharacterAttribute> GetStatsMap()
    {
        SerializedDictionary<UnitStat, CharacterAttribute> dict = new();

        return dict;
    }


}
