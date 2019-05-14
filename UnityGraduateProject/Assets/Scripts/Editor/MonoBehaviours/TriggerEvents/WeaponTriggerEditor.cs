using UnityEditor;
using TES;

[CustomEditor(typeof(WeaponTrigger))]
public class WeaponTriggerEditor : Editor
{
    SerializedObject weaponTrigger;
    SerializedProperty damage;

    private void OnEnable()
    {
        weaponTrigger = new SerializedObject(target);
        damage = weaponTrigger.FindProperty("damage");
    }
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Weapon trigger component", MessageType.Info);
        weaponTrigger.Update();
        damage.floatValue = EditorGUILayout.FloatField("Damage: ", damage.floatValue);
        weaponTrigger.ApplyModifiedProperties();
    }
}
