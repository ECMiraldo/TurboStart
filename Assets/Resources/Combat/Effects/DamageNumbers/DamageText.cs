using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DamageText : MonoBehaviour, IPooledObject
{
    [field: SerializeField] public ObjectPoolSettings poolSettings { get; private set; }
    [SerializeField] private float motion;
    [SerializeField] private float motionDuration;
    [SerializeField] private float despawnTime;

    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void SetDamage(Damage dmg, ObjectPool pool)
    {
        text.text = dmg.amount.ToString();
        transform.DOLocalMoveY(transform.position.y + motion, motionDuration);
        StartCoroutine(DespawnRoutine(pool));
    }

    private IEnumerator DespawnRoutine(ObjectPool pool)
    {
        yield return new WaitForSeconds(despawnTime);
        pool.Despawn(this);
    }
}


