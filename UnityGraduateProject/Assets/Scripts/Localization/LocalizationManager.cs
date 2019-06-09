using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LocalizationManager : MonoBehaviour, Initializable
{
    public string Language
    {
        get
        {
            if (string.IsNullOrEmpty(language))
                language = std_language;
            return language;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                language = value;
        }
    }
    private string language;
    private string std_language = "Ukrainian";
    private Localizable[] objects;
    public List<Localizable> lobjects;

    public static LocalizationManager Instance => instance;
    private static LocalizationManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    public void Initialize()
    {
        if (LoadLocalizable())
            LoadLocalization();
    }

    public void SaveLocalization()
    {
        string path = Application.streamingAssetsPath + "/Localisation/" + Language + "/Scenes/" + SceneManager.GetActiveScene().buildIndex + ".txt";
        

        List<string> writeStrings = new List<string>();
        foreach(var lobject in objects)
        {
            // ID of the element
            writeStrings.Add(lobject.id.ToString());
            // localize text
            writeStrings.Add(lobject.text);
        }
        

        File.WriteAllLines(path, writeStrings);
    }
    public void LoadLocalization()
    {
        if (!LoadLocalizable())
            return;
        Debug.Log($"Load Localization. Localizebles: {lobjects}");
        string path = Application.streamingAssetsPath + "/Localisation/" + Language + "/Scenes/" + SceneManager.GetActiveScene().buildIndex + ".txt";
        if (!File.Exists(path))
            return;
        string[] localStrings;
        localStrings = File.ReadAllLines(path);
        int objind = 0;        
        for (int i = 0; i < localStrings.Length; i+=2)
        {
            objind = int.Parse(localStrings[i]);
            lobjects.Find(obj => obj.id == objind).text = localStrings[i+1];
        }
        foreach (var localizable in lobjects)
        {
            localizable.LocalizeComponent();
        }
    }
    public bool LoadLocalizable()
    {
        objects = FindObjectsOfType<Localizable>();
        if (objects != null && objects.Length > 0)
        {
            lobjects = new List<Localizable>();
            lobjects.AddRange(objects);
            return true;
        }
        return false;
    }


}

