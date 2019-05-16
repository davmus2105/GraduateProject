using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class MainMenu : MonoBehaviour
{

	
	public Slider volumeMusic;
	public Slider volumeEffect;
	public GameObject panelSettings;
	public Dropdown ResolutionDropdown;
    public Dropdown GraphicsDropdown;
	Resolution[] res;
	
	public static bool IsActivated = false;
	public enum typeSound { music,effect };
	public typeSound type;
	AudioSource audioSource;
	
	
	
	public void Start () {
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
	public void StartGame()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void QuitGame()
	{
		Application.Quit();
	}
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

		Debug.Log("The resolution was recived");
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
