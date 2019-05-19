using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GraduateAudio
{
    [CreateAssetMenu(fileName = "AudioCollection", menuName = "AudioCollection/Create new Audio Collection")]
    public class AudioCollection : ScriptableObject
    {
        // ---------------------- Footsteps Collections ------------------------
        public AudioClip[]
                footsteps_gravel,
                footsteps_grass,
                footsteps_mud;
        // ---------------------- Ambience Collections ------------------------
        public AudioClip[]
                ambience_inForest_birdsSong;
        // ---------------------- Battle Effects Collections ------------------------
        public AudioClip[]
                battleCry,
                battle_grunts;

        public AudioClip
                get_sword,
                hide_sword;
        // ---------------------- Background Music Collections ------------------------
        public AudioClip[]
                battle_music,
                tranqul_music;

    }
}