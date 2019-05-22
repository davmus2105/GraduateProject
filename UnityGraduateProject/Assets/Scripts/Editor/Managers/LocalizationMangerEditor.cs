#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
[CustomEditor(typeof(LocalizationManager))]
public class LocalizationMangerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LocalizationManager lman = (LocalizationManager)target;
        DrawDefaultInspector();
        GUILayout.Space(20);
        lman.Language = EditorGUILayout.TextField("Language:", lman.Language);
        
        if (GUILayout.Button("LOAD LOCALIZATION"))
        {
            lman.LoadLocalization();
        }
        if (GUILayout.Button("SAVE LOCALIZATION"))
        {
            lman.SaveLocalization();
        }
        GUILayout.Space(20);
        if (GUILayout.Button("LOAD LOCALIZABLE"))
        {
            lman.LoadLocalizable();
        }
        EditorGUILayout.HelpBox("To add localizable component to all text ui objects use this button", MessageType.Info);
        if (GUILayout.Button("MASS ADD"))
        {
            MassAddLocalizableComponent();
        }
        if (GUILayout.Button("MASS REMOVE"))
        {
            MassRemoveLocalizableComponent();
        }
    }
    void MassAddLocalizableComponent()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Localizable");
        for (int i = 0; i < objects.Length; i++)
        {
            Localizable tempLoc = objects[i].GetComponent<Localizable>();
            if (tempLoc == null)
                tempLoc = objects[i].AddComponent<Localizable>();
            tempLoc.id = i;
        }
        Debug.Log("Mass Add is ended");
    }
    void MassRemoveLocalizableComponent()
    {
        Localizable[] localizables = FindObjectsOfType<Localizable>();
        foreach (var item in localizables)
        {
            DestroyImmediate(item);
        }
        Debug.Log("Mass remove is ended");
    }
}
#endif