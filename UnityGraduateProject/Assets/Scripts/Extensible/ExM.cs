using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExM
{
    public static void SetActiveGameObjects(GameObject[] list, bool active)
    {
        foreach(var go in list)
            go.SetActive(active);
    }
    public static void SetActiveGameObjects(Transform[] list, bool active)
    {
        foreach (var go in list)
            go.gameObject.SetActive(active);
    }

    // --------- GET RANDOM ITEM -----------
    public static T GetRandomItem<T>(this T[] list)
    {
        return list[Random.Range(0, list.Length - 1)];
    }
    public static T GetRandomItem<T>(this List<T> collection)
    {
        return collection[Random.Range(0, collection.Count - 1)];
    }
}
