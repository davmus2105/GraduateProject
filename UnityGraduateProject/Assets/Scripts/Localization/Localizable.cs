using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Localizable : MonoBehaviour
{
    public int id;
    public string text;

    public void LocalizeComponent()
    {
        GetComponent<Text>().text = text;
    }
}
