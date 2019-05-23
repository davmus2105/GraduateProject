using TES;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogueTrigger))]
public class DialogueTriggerEditor : Editor
{
    SerializedObject dialTrigger;
    SerializedProperty dialName;
    SerializedProperty isOneTime;
    private void OnEnable()
    {
        dialTrigger = new SerializedObject(target);
        dialName = dialTrigger.FindProperty("dialogue_name");
        isOneTime = dialTrigger.FindProperty("isOneTime");
    }
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Dialogue trigger component", MessageType.Info);
        dialTrigger.Update();               
        dialName.stringValue = EditorGUILayout.TextField("dialogue name: ", dialName.stringValue);
        isOneTime.boolValue = EditorGUILayout.Toggle("Is one time", isOneTime.boolValue);
        dialTrigger.ApplyModifiedProperties();
        EditorGUILayout.LabelField($"isUsed is {((DialogueTrigger)target).isUsed}");
    }
}
