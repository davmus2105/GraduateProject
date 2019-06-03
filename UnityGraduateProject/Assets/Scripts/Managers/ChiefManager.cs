using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ChiefManager : MonoBehaviour
{
    [Tooltip("Managers that will be added on start")]
    // managers to add on [SETUP] when game is started
    public List<string> managers;
    [Space(25)]
    [SerializeField]string filename = "managers_list.txt";
    [SerializeField]string managerspath = "/Kernel/";
    public List<string> unsettupable_scenes = new List<string>() { "Main Menu" };
    public List<int> unsettupable_index = new List<int>() { 0, 2, 3 };
    public static ChiefManager Instance=>instance;
    private static ChiefManager instance;

    private void Awake()
    {
        if (!LoadManagersList())
        {
            managers = new List<string>();
        }
        Init();
    }
    void Init()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        Initializing();
    }
    private void OnLevelWasLoaded(int level)
    {
        Initializing();
    }
    private void OnDisable()
    {
        SaveManagersList();
    }
    void Start()
    {
    }

    void Initializing()
    {
        Init();
        Debug.Log($"build index = {SceneManager.GetActiveScene().buildIndex}");
        if (unsettupable_index.Contains(SceneManager.GetActiveScene().buildIndex))
        {
            Debug.Log("Scene is contained");
            Initializable[] initializables = gameObject.GetComponents<Initializable>();
            foreach (var item in initializables)
            {
                ((MonoBehaviour)item).enabled = false;
            }
            GetComponent<AudioSource>().enabled = false;
        }
        else
        {
            Debug.Log("Initializing");
            LoadManagersList();
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
            Initializable[] initializables = gameObject.GetComponents<Initializable>();
            foreach (var item in initializables)
            {
                ((MonoBehaviour)item).enabled = true;
                item.Initialize();
            }
            GetComponent<AudioSource>().enabled = true;
        }
        
        DontDestroyOnLoad(this);
        foreach (var components in GetComponents<MonoBehaviour>())
        {
            DontDestroyOnLoad(components);
        }
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
