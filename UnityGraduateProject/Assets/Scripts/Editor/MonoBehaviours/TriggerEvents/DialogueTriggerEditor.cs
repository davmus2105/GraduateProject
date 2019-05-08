using TES;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogueTrigger))]
public class DialogueTriggerEditor : Editor
{
    SerializedObject dialTrigger;
    SerializedProperty dialName;
    private void OnEnable()
    {
        dialTrigger = new SerializedObject(target);
    }
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Dialogue trigger component", MessageType.Info);
        dialTrigger.Update();
        dialName = dialTrigger.FindProperty("dialogue_name");       
        dialName.stringValue = EditorGUILayout.TextField("dialogue name: ", dialName.stringValue);
        dialTrigger.ApplyModifiedProperties();
    }
}
