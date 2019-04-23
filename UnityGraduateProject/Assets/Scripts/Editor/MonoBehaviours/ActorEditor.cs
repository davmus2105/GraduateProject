using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Actor))]
public class ActorEditor : Editor
{
    Actor actor;
    bool showcharinfo, showhealth, showinv;

    private void OnEnable()
    {
        actor = (Actor)target;

    }

    public override void OnInspectorGUI()
    {
        actor = (Actor)target;
        GUILayout.BeginVertical();
        EditorGUI.indentLevel++;
        showcharinfo = EditorGUILayout.Foldout(showcharinfo, "CharInfoComponent");
        if (showcharinfo)
        {
            if (actor.charinfo != null)
            {
                EditorGUILayout.LabelField("Characters name: ", actor.charinfo.GetCharacterName());
            }
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("ADD"))
                {
                    actor.charinfo = new CharacterInfoComponent(actor, "Player");
                }
                if (GUILayout.Button("LOAD"))
                {

                }
                GUILayout.EndHorizontal();
            }
        }
        showhealth = EditorGUILayout.Foldout(showhealth, "Health");
        if (showhealth)
        {
            if (actor.health != null)
            {
              //  EditorGUILayout.LabelField("Health: ", actor.health);
            }
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("ADD"))
                {
                    actor.health = new HealthComponent(actor);
                }
                if (GUILayout.Button("LOAD"))
                {

                }
                GUILayout.EndHorizontal();
            }
        }
        showinv = EditorGUILayout.Foldout(showinv, "Inventory");
        if (showinv)
        {
            if (actor.inventory != null)
            {

            }
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("ADD"))
                {
                    actor.inventory = new InventoryComponent(actor);
                }
                if (GUILayout.Button("LOAD"))
                {

                }
                GUILayout.EndHorizontal();
            }
        }
        EditorGUI.indentLevel--;
        GUILayout.EndVertical();
    }
}