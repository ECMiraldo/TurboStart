using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static T GetRandomFromList<T>(this List<T> collection)
    {
        return collection[UnityEngine.Random.Range(0, collection.Count)];
    }

    public static List<T> GetNumberRandomFromList<T>(this List<T> collection, int n)
    {
        List<T> copy = new(collection);

        for (int i = copy.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);

            (copy[i], copy[j]) = (copy[j], copy[i]);
        }

        n = Mathf.Min(n, copy.Count);

        return copy.GetRange(0, n);
    }

    public static string FormatTime(long totalSeconds)
    {
        long hours = totalSeconds / 3600;
        long minutes = (totalSeconds % 3600) / 60;
        long seconds = totalSeconds % 60;

        if (hours > 0)
            return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
        else
            return $"{minutes:D2}:{seconds:D2}";
    }


}
