using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSecond : MonoBehaviour
{
    public string loadName;
    public string unloadName;

    private void OnTriggerEnter(Collider coll) 
    {
        if(loadName != "")
        {
            ManagerScript.Instance.Load(loadName);
        }
        if(unloadName !="")
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
