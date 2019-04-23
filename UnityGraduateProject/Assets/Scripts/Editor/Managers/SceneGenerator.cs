using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace PRJ_ZHMURKI
{
[InitializeOnLoad]
public class SceneGenerator 
{
    static SceneGenerator()
    {
        EditorSceneManager.newSceneCreated += SceneCreating;
    }

    public static void SceneCreating(Scene scene, NewSceneSetup setup, NewSceneMode mode)
    {
        
        GameObject.DestroyImmediate(GameObject.Find("Main Camera"));
        GameObject.DestroyImmediate(GameObject.Find("Directional Light"));
        
        var setupFolder = new GameObject("[SETUP]").transform;
        var sceneFolder = new GameObject("[SCENE]").transform;
        var uiFolder = new GameObject("[UI]").transform;

        new GameObject("===== STATIC =====").transform.SetParent(sceneFolder);
        new GameObject("===== DYNAMIC =====").transform.SetParent(sceneFolder);

        Debug.Log("Scene was created");
    }
}
}
