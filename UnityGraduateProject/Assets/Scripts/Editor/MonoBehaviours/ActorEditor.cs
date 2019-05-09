#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Actor))]
public class ActorEditor : Editor
{
    Actor actor;
    bool ShowCharInfo {
        get { return ShowCharInfo; }
        set
        {
            if (value)
            {
                if (actor.charinfo == null)
                    actor.charinfo = new CharacterInfoComponent(actor, 1);
            }
            ShowCharInfo = value;
        }
    }
    bool ShowHealth
    {
        get { return ShowHealth; }
        set
        {
            if (value)
            {
                if (actor.health == null)
                    actor.health = new HealthComponent(actor);
            }
            ShowHealth = value;
        }
    }
    bool ShowInv
    {
        get { return ShowInv; }
        set
        {
            if (value)
            {
                if (actor.inventory == null)
                {
                    actor.inventory = new InventoryComponent(actor);
                }                
            }
            ShowInv = value;
        }
    }
    float health, max_health;

    private void OnEnable()
    {
        actor = (Actor)target;

    }

    public override void OnInspectorGUI()
    {
        actor = (Actor)target;
        GUILayout.BeginVertical();
        EditorGUI.indentLevel++;
        ShowCharInfo = EditorGUILayout.Foldout(ShowCharInfo, "CharInfoComponent");
        if (ShowCharInfo)
        {
            EditorGUILayout.LabelField("Characters name: ", actor.charinfo.GetCharacterName());
        }
        ShowHealth = EditorGUILayout.Foldout(ShowHealth, "Health");
        if (ShowHealth)
        {            
            EditorGUILayout.LabelField("Health: ", actor.health.GetHealth().ToString() + "/" + actor.health.GetMaxHealth().ToString());
            EditorGUILayout.HelpBox("You can set your values to health and to max health", MessageType.Info);
            // Set health and max health
            // ---- SET health
            GUILayout.BeginHorizontal();
            health = EditorGUILayout.FloatField("Set health", health);
            if (GUILayout.Button("SET"))
                actor.health.SetHealth(health);
            GUILayout.EndHorizontal();
            // ---- SET max health
            GUILayout.BeginHorizontal();
            max_health = EditorGUILayout.FloatField("Set max health", max_health);
            if (GUILayout.Button("SET"))
                actor.health.SetMaxHealth(max_health);
            GUILayout.EndHorizontal();           
        }
        ShowInv = EditorGUILayout.Foldout(ShowInv, "Inventory");
        if (ShowInv)
        {
            
            
        }
        EditorGUI.indentLevel--;
        GUILayout.EndVertical();
    }
}
#endif