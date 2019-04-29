using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExM
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

}
