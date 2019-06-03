using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Audio;


public class PauseMenu : MonoBehaviour, Initializable
{

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
    public Slider Music;
    public Slider Effects;
    public Toggle Fullscreen;

    // Additional parameters
    bool wasInDialogue;
    public bool isFull = true;


    private void Awake()
    {
        Instance = this;
    }
    
    public void Start() {

        Initialize(); 
	}
    
    public void Initialize()
    {
        pauseMenuUI = GameObject.Find("[UI]").transform.Find("PauseMenu").gameObject;
        panelSettings = pauseMenuUI.transform.Find("Panel_Settings").gameObject;        
        resume_button = pauseMenuUI.transform.Find("Resume_button").gameObject;
        menu_button = pauseMenuUI.transform.Find("Menu_button").gameObject;
        quit_button = pauseMenuUI.transform.Find("Quit_button").gameObject;
        settings_button = pauseMenuUI.transform.Find("Settings_button").gameObject;
        ResolutionDropdown = panelSettings.transform.Find("Resolution Dropdown").GetComponent<Dropdown>();
        GraphicsDropdown = panelSettings.transform.Find("Graphics Dropdown").GetComponent<Dropdown>();
        Fullscreen = panelSettings.transform.Find("FullScreen Togle").GetComponent<Toggle>();
        Resolution();
        wasInDialogue = false;
        // Buttons onclick init
        resume_button.GetComponent<Button>().onClick.AddListener(delegate { Resume(); });
        menu_button.GetComponent<Button>().onClick.AddListener(delegate { LoadMenu(); });
        quit_button.GetComponent<Button>().onClick.AddListener(delegate { QuitGame(); });
        settings_button.GetComponent<Button>().onClick.AddListener(delegate { Settings(); });
        ResolutionDropdown.GetComponent<Dropdown>().onValueChanged.AddListener(delegate { SetRes(); });
        GraphicsDropdown.GetComponent<Dropdown>().onValueChanged.AddListener(delegate { SetQuality(); });
        Fullscreen.GetComponent<Toggle>().onValueChanged.AddListener(delegate { SetFullScreen(); });
        // back button in settings
        panelSettings.transform.Find("Back").GetComponent<Button>().onClick.AddListener(delegate { Back(); });
        
        //panelSettings.transform.Find("Slider Music").GetComponent<Slider>().onValueChanged.AddListener(delegate { SetMusicVoll(45); });
       // panelSettings.transform.Find("Slider Effect").GetComponent<Slider>().onValueChanged.AddListener(delegate { SetEffectVoll(40); });

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
        if (DialogueManager.Instance.inDialogue)
        {
            wasInDialogue = true;
            DialogueManager.Instance.SetActiveDialoguePanel(false);
        }
        else
            wasInDialogue = false;
        ExM.SetActiveCursor(true);

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
        ExM.SetActiveCursor(false);
        if (wasInDialogue)
            DialogueManager.Instance.SetActiveDialoguePanel(true);

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
    public void SetFullScreen()
	{
       
        if(Fullscreen.isOn)
        {
            Screen.fullScreen = true;
            Debug.Log("SETFULL screen on");
            return;
        }
        else
        {
            Screen.fullScreen = false;
            return;
        }
       
	}
    public void SetQuality()
	{
	QualitySettings.SetQualityLevel(GraphicsDropdown.value);
        Debug.Log("set quality" + GraphicsDropdown.value);
		
	}
}
