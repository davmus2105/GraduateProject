using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerScript : MonoBehaviour
{
    public string loadName;
    public string unloadName;
    public static ManagerScript Instance{set;get;}

    private void Awake() 
    {
        Instance = this;
        Load("Map");
        Load("village"); 

    }

    public void Load(string sceneName)
    {
        if (!SceneManager.GetSceneByName(sceneName).isLoaded)
            {
                SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            }
        }

    public void Unload (string sceneName)
    {
        if(SceneManager.GetSceneByName(sceneName).isLoaded)
            SceneManager.UnloadScene(sceneName);
    
    }
    

    private void OnTriggerEnter(Collider coll)
    {
        if (loadName != "")
        {
            ManagerScript.Instance.Load(loadName);
        }
        if (unloadName != "")
        {
            StartCoroutine("UnloadScene");
        }

    }
    IEnumerator UnloadScene()
    {
        yield return new WaitForSeconds(.10f);
        ManagerScript.Instance.Unload(unloadName);
    }


}
