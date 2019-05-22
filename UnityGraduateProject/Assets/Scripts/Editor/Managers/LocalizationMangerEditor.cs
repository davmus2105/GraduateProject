#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
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
    }
}
#endif