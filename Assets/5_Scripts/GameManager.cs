using Persistence;
using UnityEngine;
using UnityUtils;

public class GameManager : Singleton<GameManager>
{
    [field: SerializeField] public static ProfileData ProfileData => SaveLoadSystem.Instance.data;
}
