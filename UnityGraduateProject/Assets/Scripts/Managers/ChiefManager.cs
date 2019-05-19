using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ChiefManager : MonoBehaviour
{
    [Tooltip("Managers that will be added on start")]
    // managers to add on [SETUP] when game is started
    public List<string> managers;
    [Space(25)]
    [SerializeField]string filename = "managers_list.txt";
    [SerializeField]string managerspath = "/Kernel/";

    private void Awake()
    {
        if (!LoadManagersList())
        {
            managers = new List<string>();
        }
    }
    private void OnEnable()
    {
        LoadManagersList();
    }
    private void OnDisable()
    {
        SaveManagersList();
    }
    void Start()
    {
        // ----- Managers setup -----
        if (managers.Count > 0)
        {
            List<MonoBehaviour> managerexists = new List<MonoBehaviour>();
            foreach (var manager in managers)
            {
                var component = System.Type.GetType(manager);
                if (gameObject.GetComponent(component) == null)
                    gameObject.AddComponent(component);
            }
        }
        DontDestroyOnLoad(this);
        foreach(var components in GetComponents<MonoBehaviour>())
        {
            DontDestroyOnLoad(components);
        }
        // ---- UI SETUP ----

    }

    
    bool LoadManagersList()
    {
        string path = Application.streamingAssetsPath + managerspath + filename;
        string[] lines = File.ReadAllLines(path);
        if (lines == null || lines.Length == 0)
        {
            Debug.Log("Something has gone wrong");
            return false;
        }
            
        if (managers == null)
            managers = new List<string>(lines.Length);
        else
            managers.Clear();
        for (int i = 0; i < lines.Length; i++)
        {
            managers.Add(lines[i]);
        }
        return true;
    }
    void SaveManagersList()
    {
        if (managers == null || managers.Count == 0)
            return;
        string path = Application.streamingAssetsPath + managerspath + filename;
        File.WriteAllLines(path, managers);        
    }
}
