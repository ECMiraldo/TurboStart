using UnityEngine;
using Persistence;

public class InventoryUI : MonoBehaviour
{
    private PersistentData gameData;
    private void Start()
    {
        gameData = SaveLoadSystem.Instance.data;
    }

}
