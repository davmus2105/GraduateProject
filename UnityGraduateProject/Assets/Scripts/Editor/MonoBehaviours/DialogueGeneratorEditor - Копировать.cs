#if UNITY_EDITOR
using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogueGenerator))]
public class DialogueGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GUILayout.Space(15);
        DialogueGenerator e = (DialogueGenerator)target;
        int i = 0;
        foreach(var node in e.node)
        {
            node.node_id = i;
            i++;
        }
        if (GUILayout.Button("Generate Dialogue XML"))
        {
            e.Generate();
        }
        if (GUILayout.Button("LOAD"))
        {
            e.LoadDialogue();
        }
    }
}
#endif