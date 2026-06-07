using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [field: SerializeField] public float TravelSpeedMultiplier { get; private set; } = 1.0f;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
