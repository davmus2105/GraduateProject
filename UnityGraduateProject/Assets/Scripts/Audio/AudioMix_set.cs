using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMix_set : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void SetEffectVoll(float setEffVoll)
    {
        audioMixer.SetFloat("enemyVol",setEffVoll);
        audioMixer.SetFloat("humanVol",setEffVoll);
        audioMixer.SetFloat("playerVol",setEffVoll);
    }
    public void SetMusicVoll(float setMusVoll)
    {
        audioMixer.SetFloat("backgrVol",setMusVoll);
        
    }
}
