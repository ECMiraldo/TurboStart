using System.Collections.Generic;

public static class Utils
{
    public static T GetRandomFromList<T>(List<T> collection)
    {
        return collection[UnityEngine.Random.Range(0, collection.Count)];
    }

}
