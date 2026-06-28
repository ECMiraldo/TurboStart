using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DamageNumberManager : MonoBehaviour
{
    public static DamageNumberManager instance { get; private set; }
    [SerializeField] public ObjectPoolSettings damageNumberSettings;

    private ObjectPool objectPool;
    private Camera cam;
    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        instance = this;
        objectPool = new ObjectPool(new List<ObjectPoolSettings>() { damageNumberSettings }, this);
        cam = Camera.main;
    }

    public static void ShowNumber(Vector3 position, string text, float timeShown = 1.5f)
    {
        instance.StartCoroutine(instance.NumberRoutine(position, text, timeShown));
    }

    private IEnumerator NumberRoutine(Vector3 position, string text, float timeShown = 1.5f)
    {
        Vector2 screenPos = cam.WorldToScreenPoint(position);
        DamageNumber number = objectPool.Spawn(damageNumberSettings, screenPos).gameObject.GetComponent<DamageNumber>();
        number.ShowNumber(text);
        yield return new WaitForSeconds(timeShown);
        objectPool.Despawn(number);
    }
}
