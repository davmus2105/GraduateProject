using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Actor))]
public class ActorEditor : Editor
{
    SerializedObject actorSO;
    SerializedProperty health, max_health, character_name;
    Actor actor;
    float maxHealth;
    string tempname;
    private void OnEnable()
    {        
        actorSO = new SerializedObject(target);        
        character_name = actorSO.FindProperty("character_name");        
        health = actorSO.FindProperty("health");        
        max_health = actorSO.FindProperty("max_health");
        actor = (Actor)target;
    }
    
   /* public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.LabelField("Name: ", actor.CharacterName);
        EditorGUILayout.LabelField("Health: ", actor.GetHealth() + "/" + actor.GetMaxHealth());
        EditorGUILayout.HelpBox("SET VALUES:", MessageType.Info);
        // set name
        EditorGUILayout.BeginHorizontal();
        tempname = EditorGUILayout.TextField("Name: ", tempname);
        if (GUILayout.Button("SET"))
        {
            actor.CharacterName = tempname;
            tempname = "";
        }
        EditorGUILayout.EndHorizontal();
        // set max health
        EditorGUILayout.BeginHorizontal();
        maxHealth = EditorGUILayout.FloatField("Max health: ", maxHealth);
        if (GUILayout.Button("SET"))
        {
            actor.SetMaxHealth(maxHealth);
            maxHealth = 0;
        }
        EditorGUILayout.EndHorizontal();
        // set health
        EditorGUILayout.LabelField("Health");
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("-100"))
        {
            GetDamage(100f);
        }
        if (GUILayout.Button("-10"))
        {
            GetDamage(10f);
        }
        if (GUILayout.Button("+10"))
        {
            AddHealth(10f);
        }
        if (GUILayout.Button("+100"))
        {
            AddHealth(100f);
        }
        EditorGUILayout.EndHorizontal();
    }*/

    public override void OnInspectorGUI()
    {
        actorSO.Update();
        EditorGUILayout.LabelField("Name: ", character_name.stringValue);
        EditorGUILayout.LabelField("Health: ", health.floatValue + "/" + max_health.floatValue);
        EditorGUILayout.HelpBox("SET VALUES:", MessageType.Info);
        // set name
        EditorGUILayout.BeginHorizontal();
        tempname = EditorGUILayout.TextField("Name: ", tempname);
        if (GUILayout.Button("SET"))
        {            
            character_name.stringValue = tempname;
            tempname = "";
        }
        EditorGUILayout.EndHorizontal();
        // set max health
        EditorGUILayout.BeginHorizontal();
        maxHealth = EditorGUILayout.FloatField("Max health: ", maxHealth);
        if (GUILayout.Button("SET") && maxHealth > 0)
        {
            max_health.floatValue = maxHealth;
            maxHealth = 0;
        }
        EditorGUILayout.EndHorizontal();
        // set health
        EditorGUILayout.LabelField("Health");
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("-100"))
        {
            GetDamage(100f);
        }
        if (GUILayout.Button("-10"))
        {
            GetDamage(10f);
        }
        if (GUILayout.Button("+10"))
        {
            AddHealth(10f);
        }
        if (GUILayout.Button("+100"))
        {
            AddHealth(100f);
        }
        EditorGUILayout.EndHorizontal();
        actorSO.ApplyModifiedProperties();
    }

    public void AddHealth(float _health)
    {
        if (_health <= 0)
            return;
        health.floatValue += _health;
        if (health.floatValue > max_health.floatValue)
            health.floatValue = max_health.floatValue;
    }
    public void GetDamage(float _damage)
    {
        if (_damage <= 0)
            return;
        health.floatValue -= _damage;
        if (health.floatValue <= 0)
        {
            actor.connectedBehaviour.Die();
        }
    }
}
