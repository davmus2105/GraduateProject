using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Audio
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
        // --------------------------------------------------------------
        private float _totalAudioVolume;
        private float _totalAudioPitch;
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
            DontDestroyOnLoad(gameObject);
        }
        #endregion
    }
}