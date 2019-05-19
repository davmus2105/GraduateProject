using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
namespace GraduateAudio
{
    [RequireComponent(typeof(AudioListener))]
    public class AudioManager : MonoBehaviour
    {
        #region Properties and Fields
        public float TotalAudioVolume
        {
            get
            {
                return _totalAudioVolume;
            }
            set
            {
                _totalAudioVolume = Mathf.Clamp(value, -80, 20);
            }
        }
        public float TotalAudioPitch
        {
            get
            {
                return _totalAudioPitch;
            }
            set
            {
                _totalAudioPitch = Mathf.Clamp(value, 1, 1000);
            }
        }
        
        // --------------------------------------------------------------
        public static AudioManager instance = null;
        public static float footstep_volume = 0.1f;
        public static float battleEffects_volume = 1f;
        public bool isInBattle;
        public AudioCollection audioCollection;
        // --------------------------------------------------------------
        private float _totalAudioVolume;
        private float _totalAudioPitch;
        private AudioSource backgroundsource;
        // --------------------------------------------------------------
        #endregion
        #region Methods
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        private void Start()
        {
            isInBattle = false;
            backgroundsource = GetComponent<AudioSource>();
            if (!backgroundsource)
                backgroundsource = gameObject.AddComponent<AudioSource>();
            if (audioCollection == null)
                audioCollection = GameObject.FindGameObjectWithTag("Player").transform.Find("Model").GetComponent<PlayerAnimatorController>().audio_collection;
        }
        private void Update()
        {
            if (!backgroundsource.isPlaying)
                BackgroundChoose();
            Debug.Log("is Playing: " + backgroundsource.isPlaying);
        }
        
        public void BackgroundChoose()
        {
            if (isInBattle)
            {
                backgroundsource.clip = audioCollection.battle_music.GetRandomItem();
            }
            else
            {
                backgroundsource.clip = audioCollection.tranqul_music.GetRandomItem();
            }
            backgroundsource.Play();
        }

        #endregion
    }
}