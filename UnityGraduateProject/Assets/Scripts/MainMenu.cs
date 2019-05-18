using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;



public class MainMenu : MonoBehaviour
{

	//public SoundProperties soundProperties;
	public Slider volumeMusic;
	public Slider volumeEffect;
    public Slider SliderOfLoad; //child slider
    public Text progressText; //the text
    public GameObject panelSettings;
    public GameObject Slider_of_load; // the parent slider
    public GameObject Progress; // the parent progress obj
    public Dropdown ResolutionDropdown;
    public Dropdown GraphicsDropdown;
	Resolution[] res;
	
	public static bool IsActivated = false;
	public enum typeSound { music,effect }; // you can select type 
	public typeSound type;
	AudioSource audioSource;
    
	
	
	/*private void Awake() {
		
	  soundProperties.volumeEffect = 0.5f;
	  soundProperties.volumeMusic = 0.5f;
	}*/
	public void Start () {

        Slider_of_load.SetActive(false); // the emptu gameobj for slider (parent)
        Progress.SetActive(false);
        audioSource = GetComponent<AudioSource>();
		Resolution();

	}

	void Update () {
		
	}

	public void Settings()
	{
		panelSettings.SetActive(true);
		/* if(IsActivated)
		{
			
			IsActivated = true;
		} else
		{
			panelSettings.SetActive(false);
			IsActivated = false;
		}
	*/
	}
	 
	
	public void Apply()
	{	audioSource.volume = volumeMusic.value;
		/*/soundProperties.volumeMusic = volumeMusic.value;
		soundProperties.volumeEffect = volumeEffect.value;*/
	}
	public void Back()
	{
		panelSettings.SetActive(false);
	}
	
    public void LoadScene(int sceneIndex) // loading the next scene by index 
    {
        Time.timeScale = 1f;
        StartCoroutine(LoadAsync(sceneIndex));
    }
    IEnumerator LoadAsync(int sceneIndex) // ассинхронная загрузка сцены и калькуляция прогресса
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        Slider_of_load.SetActive(true); // the emptu gameobj for slider (parent)
        Progress.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            SliderOfLoad.value = progress; // the value of slider (child)
            progressText.text = string.Format("{0:0}%",progress * 100);
                if (SliderOfLoad.value == 1)
                {
                progressText.text = "Wait please...";
                }
            yield return null;
        }


    }

    public void QuitGame()
	{
		Application.Quit();
	}
	public void Resolution()
	{
	
    
		GraphicsDropdown.ClearOptions(); //clear previos graphics levels
		GraphicsDropdown.AddOptions(QualitySettings.names.ToList()); // create new graphics levels
		GraphicsDropdown.value = QualitySettings.GetQualityLevel(); // set the current graphics level


        Resolution [] resolution = Screen.resolutions; 
        res = resolution.Distinct().ToArray();
        string[] strRes = new string[res.Length];
        for(int i = 0; i < res.Length; i++)
        {
            strRes[i] = res[i].ToString();
        } 
        ResolutionDropdown.ClearOptions();
        ResolutionDropdown.AddOptions(strRes.ToList()); // add new values of resolution dropdown
		ResolutionDropdown.value = res.Length - 1;

        Screen.SetResolution(res[res.Length-1].width, res[res.Length - 1].height, true); // set last value of resolution for your screen

		Debug.Log("Get rosolutions");
	}
	public void SetRes ()
    {
       Screen.SetResolution(res[ResolutionDropdown.value].width, res[ResolutionDropdown.value].height,true); // apply the selected resolution

    }
    public void SetFullScreen( bool isFull)
	{
		Screen.fullScreen = isFull; // set fullscrean mode
	}
    public void SetQuality()
	{
	    QualitySettings.SetQualityLevel(GraphicsDropdown.value);
		
	}
}
