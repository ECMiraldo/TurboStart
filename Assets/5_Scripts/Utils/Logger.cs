using UnityEngine;
using UnityUtils;

#if UNITY_EDITOR
public class Logger : Singleton<Logger>
{
    [SerializeField] private bool logScenes = true;
    [SerializeField] private bool logPersistence = true;
    public static void LogScenes(string message)
    {
        if (Instance.logScenes) Debug.Log(message);
    }

    public static void LogPersistence(string message)
    {
        if (Instance.logPersistence) Debug.Log(message); 
    }
}

#endif
