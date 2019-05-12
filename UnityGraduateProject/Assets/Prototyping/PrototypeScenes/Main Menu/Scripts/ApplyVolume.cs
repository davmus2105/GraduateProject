using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyVolume : MonoBehaviour {

	public SoundProperties soundProperties;
	public enum typeSound { music,effect };
	public typeSound type;
	
	AudioSource audioSource;
	void Start () {
		audioSource = GetComponent<AudioSource>();
			
	}
	
	private void Update() {
		if(type == typeSound.effect)
		{
			audioSource.volume = soundProperties.volumeEffect;

		}
		else if (type == typeSound.music)
		{
			audioSource.volume = soundProperties.volumeMusic;
		}
	}
	
}
