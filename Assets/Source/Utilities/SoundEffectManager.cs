using ElementalEngagement.Favor;
using ElementalEngagement.Player;
using JetBrains.Annotations;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ElementalEngagement.Utilities
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundEffectManager : MonoBehaviour
    {
        [Tooltip("The List of Sound Effects this object can play")]
        [SerializeField] List<AudioClip> audioClips = new List<AudioClip>();

        // Track the audio source of the music
        private static AudioSource audioSource = null;

        // Tracks the instance of this.
        private static SoundEffectManager instance = null;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                audioSource = GetComponent<AudioSource>();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void playRandomSound()
        {
            audioSource.PlayOneShot(GetRandomClip(audioClips));
        }

        AudioClip GetRandomClip(List<AudioClip> clips) => clips[UnityEngine.Random.Range(0, clips.Count - 1)];
    }
}