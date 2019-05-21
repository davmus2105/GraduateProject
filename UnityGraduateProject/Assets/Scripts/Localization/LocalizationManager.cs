using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
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

    public static LocalizationManager Instance => instance;
    private static LocalizationManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
}

