using UnityEngine;
using UnityUtils;


public static class Logger 
{
    [SerializeField] private static bool logScenes = true;
    [SerializeField] private static bool logPersistence = true;
    [SerializeField] private static bool logGrid = true;
    public static void LogScenes(string message)
    {
        if (logScenes) Debug.Log(message);
    }

    public static void LogPersistence(string message)
    {
        if (logPersistence) Debug.Log(message); 
    }

    public static void LogGrid(string message)
    {
        if (logGrid) Debug.Log(message);
    }
}
