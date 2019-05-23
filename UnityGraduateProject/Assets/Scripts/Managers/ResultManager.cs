using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    public Dictionary<string, int> results; // dialogue name and its result
    public void SaveResult(string dial_name, int dial_result)
    {
        if (results == null)
            results = new Dictionary<string, int>();
        else
        {
            if (results.ContainsKey(dial_name))
            {
                if (results[dial_name] < dial_result) results[dial_name] = dial_result;
                return;
            }
        }
        results.Add(dial_name, dial_result);
    }
    public int GetResult(string dial_name)
    {
        if (results == null || results.Count == 0 || !results.ContainsKey(dial_name))
            return 0;
        else
            return results[dial_name];
    }

    public static ResultManager Instance => instance;
    private static ResultManager instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        results = new Dictionary<string, int>();
    }
}
