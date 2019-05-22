using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour {

    public static PauseMenu Instance;
    [Header("   Pause Menu Objects")]
    public GameObject panelSettings;
	public GameObject pauseMenuUI;
	public GameObject resume_button;
	public GameObject menu_button;
	public GameObject quit_button;
	public GameObject settings_button;
    [Header("   Screen settings")]
    public Dropdown ResolutionDropdown;
    public Dropdown GraphicsDropdown;
    public static bool GameIsPaused = false;
    Resolution[] res;
    [Header("   Audio settings")]
    public AudioMixer audioMixer;


    private void Awake()
    {
        Instance = this;
    }
    
    public void Start() {

        Resolution();  
	}
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if(GameIsPaused)
			{
				Resume();
			}
			else
			{
				Pause();
			}
		}
	}

    //-------------------------BUTTONS IN pause menu--------------------------------
    void Pause() // stop the game and call menu
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

        panelSettings.SetActive(false);

        resume_button.SetActive(true);
        settings_button.SetActive(true);
        menu_button.SetActive(true);
        quit_button.SetActive(true);

    }
    public void Resume()
	{
		pauseMenuUI.SetActive(false);
		Time.timeScale = 1f;
		GameIsPaused = false;

	}
    public void Settings() // hide manu buttons and show panel settings
    {
        resume_button.SetActive(false);
        settings_button.SetActive(false);
        menu_button.SetActive(false);
        quit_button.SetActive(false);

        panelSettings.SetActive(true);

    }
            public void Back()
            {

                resume_button.SetActive(true);
                settings_button.SetActive(true);
                menu_button.SetActive(true);
                quit_button.SetActive(true);
                panelSettings.SetActive(false);
            }
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    
    
    //-------------------------AUDIOMIXER settings--------------------------------
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

    //-------------------------SCREEN settings--------------------------------
    public void Resolution()
	{
		GraphicsDropdown.ClearOptions(); 
		GraphicsDropdown.AddOptions(QualitySettings.names.ToList()); 
		GraphicsDropdown.value = QualitySettings.GetQualityLevel();


        Resolution [] resolution = Screen.resolutions; 
        res = resolution.Distinct().ToArray();
        string[] strRes = new string[res.Length];
        for(int i = 0; i < res.Length; i++)
        {
            strRes[i] = res[i].ToString();
        } 
        ResolutionDropdown.ClearOptions();
        ResolutionDropdown.AddOptions(strRes.ToList());
		ResolutionDropdown.value = res.Length - 1;

        Screen.SetResolution(res[res.Length-1].width, res[res.Length - 1].height, true);
		Debug.Log("Get resolurion");
	}
	public void SetRes ()
    {
       Screen.SetResolution(res[ResolutionDropdown.value].width, res[ResolutionDropdown.value].height,true);

    }
    public void SetFullScreen( bool isFull)
	{
		Screen.fullScreen = isFull;
	}
    public void SetQuality()
	{
	QualitySettings.SetQualityLevel(GraphicsDropdown.value);
		
	}
}
