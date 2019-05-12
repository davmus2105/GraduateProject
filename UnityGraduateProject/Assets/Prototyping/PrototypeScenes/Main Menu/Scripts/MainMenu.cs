using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

	public SoundProperties soundProperties;
	public Slider volumeMusic;
	public Slider volumeEffect;
	public GameObject panelSettings;
	public static bool IsActivated = false;
	
	private void Awake() {
		
	  soundProperties.volumeEffect = 0.5f;
	  soundProperties.volumeMusic = 0.5f;
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
	{
		soundProperties.volumeMusic = volumeMusic.value;
		soundProperties.volumeEffect = volumeEffect.value;
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
}
