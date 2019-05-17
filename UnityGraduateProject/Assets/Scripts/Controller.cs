using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour

{
    public GameObject Button_Load_Scene;
    public GameObject Slider_of_load;
    public Slider TheSlider;
    // Start is called before the first frame update
    public void LoadScene( int sceneIndex)
    {
        StartCoroutine(LoadAsync (sceneIndex));
    }

    IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        Button_Load_Scene.SetActive(false);
        Slider_of_load.SetActive(true);
        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress/.9f);
            TheSlider.value = progress;
            yield return null;
        }

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
