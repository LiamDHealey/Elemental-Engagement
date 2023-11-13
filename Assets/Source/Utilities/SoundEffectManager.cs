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

        [Tooltip("Sets the effects to loop or play once")]
        [SerializeField] bool loop;

        [Tooltip("If looping, defines how often to play the sound")]
        [SerializeField] float interval;

        [Tooltip("If using a duration, defines how long to play the sound")]
        [SerializeField] float duration;

        [Tooltip("Check this box to play the sound for the duration/begin looping if applicable on start")]
        [SerializeField] bool playOnStart;

        [Tooltip("How Long to fade out the sound to play")]
        [SerializeField]private float fadeOutDuration;

        private bool continuePlaying = false;

        // Track the audio source of the music
        private static AudioSource audioSource = null;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }


        private void Start()
        {
            if (playOnStart) 
            {
                PlayForDuration();
            }
            else if(loop)
            {
                PlayOnInterval(); ;
            }
        }

        public void PlayForDuration()
        {
            PlayRandomSound();
            StartCoroutine(StopAfterDelay(duration));
        }

        public void PlayRandomSound()
        {
            audioSource.PlayOneShot(GetRandomClip(audioClips));
        }

        public void PlayOnInterval()
        {
            AudioClip clipToPlay = GetRandomClip(audioClips);
            continuePlaying = true;
            StartCoroutine(EffectOnInterval(clipToPlay));
        }

        public void stopPlayOnInterval()
        {
            continuePlaying = false;
        }

        public void stopSound()
        {
            StartCoroutine(StopAfterDelay(0));
        }

        IEnumerator StopAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            float startVolume = audioSource.volume;
            Debug.Log("Starting At " + startVolume);
            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVolume * Time.deltaTime / fadeOutDuration;
                Debug.Log("Current volume during fade is: " + audioSource.volume);
                yield return null;
            }


            audioSource.Stop();
            audioSource.volume = startVolume;
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