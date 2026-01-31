using UnityEngine;






public class GridAttackComponent : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private int range = 2; // grid distance


    private float lastAttackTime;


    public bool IsInRange(Vector2Int from, Vector2Int to)
    {
        int dx = Mathf.Abs(from.x - to.x);
        int dy = Mathf.Abs(from.y - to.y);

        return Mathf.Max(dx, dy) <= range;
    }


    public bool CanAttack()
    {
        return Time.time >= lastAttackTime + attackCooldown;
    }


    public void PerformAttack(GridHealthComponent target)
    {
        if (!CanAttack()) return;


        lastAttackTime = Time.time;
        target.TakeDamage(damage);
    }
}
