using System.Collections.Generic;

public static class Utils
{
    public static T GetRandomFromList<T>(List<T> collection)
    {
        return collection[UnityEngine.Random.Range(0, collection.Count)];
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
