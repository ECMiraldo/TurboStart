using Persistence;
using UnityEngine;
using UnityUtils;

public class GameManager : Singleton<GameManager>
{
    [field: SerializeField] public ProfileData profileData { get; set; }


    private void Start()
    {
        profileData = SaveLoadSystem.Instance.data;
    }
}
