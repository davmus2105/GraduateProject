using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSceneManager : MonoBehaviour
{
    [Tooltip("How long will you see the image_finale (in sec)")]
    public float bg_duration = 5f;
    UnityEngine.UI.Image img;

    void Start()
    {
        StartCoroutine("QuitAfter");
        img = transform.Find("BG").Find("Image").gameObject.GetComponent<UnityEngine.UI.Image>();
    }

    void Quit()
    {
        ExM.SetActiveCursor(true);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    IEnumerator QuitAfter()
    {
        yield return new WaitForSeconds(bg_duration);
        Quit();
    }
}
