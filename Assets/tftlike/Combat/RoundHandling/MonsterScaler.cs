using UnityEngine;


public class ScaledStats
{
    public int hp;
    public int damage;
    public float attackSpeed;
    public int range;
}
public static class MonsterScaler
{
    public static ScaledStats Scale(
        EnemyDataSO data,
        float difficulty)
    {
        return new ScaledStats
        {
            hp = Mathf.RoundToInt(data.baseHP * difficulty),
            damage = Mathf.RoundToInt(data.baseDamage * difficulty),
            attackSpeed = data.attackSpeed,
            range = data.attackRange
        };
    }
}
