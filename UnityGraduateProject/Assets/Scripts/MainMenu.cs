using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Audio;




public class MainMenu : MonoBehaviour
{

    
    
    [Header("   Loading  process")]
    public GameObject Slider_of_load;   // the parent slider
    public Slider SliderOfLoad;         //the child slider
    public GameObject Progress;         // the parent progress obj
    public Text progressText;          //the child text
    [Header("   Main Menu Buttons")]
    [SerializeField]
    private GameObject Start_button; // to hide
    [SerializeField]
    private GameObject Settings_button; // to hide
    [SerializeField]
    private GameObject Quit_bitton; // to hide
    [Header("   Screen settings")]
    public GameObject panelSettings;
    public Dropdown ResolutionDropdown;
    public Dropdown GraphicsDropdown;
    Resolution[] res;
    [Header("   Audio Settings")]
    public AudioMixer audioMixer;

    public bool SettingsActive = false;
	
	public void Start () {

        Slider_of_load.SetActive(false); // the emptu gameobj for slider (parent)
        Start_button.SetActive(true);
        Settings_button.SetActive(true);
        Quit_bitton.SetActive(true);
        Progress.SetActive(false);
		Resolution();
	}

    //-------------------------BUTTONS IN main menu--------------------------------
    public void Settings()
	{
        
        panelSettings.SetActive(true);
        
        Start_button.SetActive(false);
        Settings_button.SetActive(false);
        Quit_bitton.SetActive(false);
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
	        public void Back()
	        {
		        panelSettings.SetActive(false);
                Start_button.SetActive(true);
                Settings_button.SetActive(true);
                Quit_bitton.SetActive(true);
	        }
	
    public void LoadScene(int sceneIndex) // loading the next scene by index 
    {
        Time.timeScale = 1f;
        StartCoroutine(LoadAsync(sceneIndex));
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    //-------------------------LOADING PROGRESS slider settings--------------------------------
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
                progressText.text = "ЗАЧЕКАЙТЕ";
                
                }
            yield return null;
        }


    }
    
    //-------------------------SCREEN settings--------------------------------
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
		Screen.fullScreen = isFull;
        //Screen.SetResolution(res[ResolutionDropdown.value - 3].width, res[ResolutionDropdown.value - 3].height, true);// set fullscrean mode

    }
    public void SetQuality()
	{
	    QualitySettings.SetQualityLevel(GraphicsDropdown.value);
        Debug.Log("set quality" + GraphicsDropdown.value);

    }
    //-------------------------AUDIO Mixer settings--------------------------------
    public void SetEffectVoll(float setEffVoll) //to set it in inspector(On Value Changed) select Dynamic float
    {
        audioMixer.SetFloat("enemyVol", setEffVoll);
        audioMixer.SetFloat("humanVol", setEffVoll);
        audioMixer.SetFloat("playerVol", setEffVoll);
    }
    public void SetMusicVoll(float setMusVoll) //to set it in inspector (On Value Changed) select Dynamic float
    {
        audioMixer.SetFloat("backgrVol", setMusVoll);

    }
}
