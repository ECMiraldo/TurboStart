using UnityEngine;

public static class GridConstants
{
    public static Team GetOpposite(this Team self)
    {
        if (self == Team.Player) return Team.Enemy;
        else if (self == Team.Enemy) return Team.Player;
        else return Team.Neutral;
    }
}



public enum Team : byte
{
    Player,
    Enemy,
    Neutral
}

public class UnitStats : MonoBehaviour
{
    public Team team;
    public int maxHealth = 100;
    public int currentHealth = 100;
    public int attackDamage = 10;
    public float attackSpeed = 2.0f;
    public int attackRange = 1;


    //Attacks
    public float lastAttackTime = 0;
}
