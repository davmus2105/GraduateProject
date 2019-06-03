using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalSceneManager : MonoBehaviour
{
    [Tooltip("How long will you see the image_finale (in sec)")]
    public float bg_duration = 5f;

    void Start()
    {
        StartCoroutine("QuitAfter");
    }

    void Quit()
    {
        Application.Quit();
    }

    IEnumerator QuitAfter()
    {
        yield return new WaitForSeconds(bg_duration);
        Quit();
    }

}
