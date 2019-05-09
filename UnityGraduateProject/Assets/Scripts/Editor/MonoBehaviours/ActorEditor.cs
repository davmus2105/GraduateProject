using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Actor))]
public class ActorEditor : Editor
{
    SerializedObject actor;
    private void OnEnable()
    {
        actor = new SerializedObject(target);
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
