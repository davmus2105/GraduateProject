#if UNITY_EDITOR
using System.Collections;
using UnityEngine;
using UnityEditor;
using QuestSystem;

[CustomEditor(typeof(QuestGenerator))]
public class QuestGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GUILayout.Space(15);
        QuestGenerator qgen = (QuestGenerator)target;
        if (GUILayout.Button("Generate Quest XML"))
        {
            qgen.Generate();
        }
        if (GUILayout.Button("LOAD"))
        {
            qgen.Load();
        }
    }
}
#endif