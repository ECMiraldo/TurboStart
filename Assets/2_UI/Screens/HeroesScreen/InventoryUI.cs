using UnityEngine;
using Persistence;

public class InventoryUI : MonoBehaviour
{
    private ProfileData gameData;
    private void Start()
    {
        gameData = GameManager.ProfileData;
    }

}
