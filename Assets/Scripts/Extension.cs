using System.Collections;
using UnityEngine;

public static class Extension
{
    public static T GetRandomElement<T>(this IList list)
    {
        if (list == null || list.Count == 0)
        {
            return default(T);
        }

        int randomIndex = Random.Range(0, list.Count - 1);
        
        return (T)(list[randomIndex]);
    }
}