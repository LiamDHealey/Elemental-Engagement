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

        [Tooltip("If using an interval, defines how often to play the sound")]
        [SerializeField] float interval;

        private bool continuePlaying = false;

        // Track the audio source of the music
        private static AudioSource audioSource = null;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlayRandomSound()
        {
            audioSource.PlayOneShot(GetRandomClip(audioClips));
        }

        public void PlayOnInterval()
        {
            AudioClip clipToPlay = GetRandomClip(audioClips);
            continuePlaying = false;
            StartCoroutine(EffectOnInterval(clipToPlay));

        }

        public void stopPlayOnInterval()
        {
            continuePlaying = false;
        }

        IEnumerator EffectOnInterval(AudioClip clipToPlay)
        {
            while(continuePlaying)
            {
                audioSource.PlayOneShot(clipToPlay);
                yield return new WaitForSeconds(interval);
            }
        }

        AudioClip GetRandomClip(List<AudioClip> clips) => clips[UnityEngine.Random.Range(0, clips.Count)];
    }
}