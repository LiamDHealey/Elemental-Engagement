using ElementalEngagement.Favor;
using ElementalEngagement.Player;
using JetBrains.Annotations;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

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
        private bool currentlyPlaying = false;
        private float startingVolume = 0f;

        [Tooltip("The Exact Name of the exposed volume parameter from the AudioMixer that the source is connected to")]
        [SerializeField] private string volumeParam = "godPowerVolume";

        // Track the audio source of the music
        private AudioSource audioSource = null;

        private AudioMixer audioMixer = null;

        private void OnDestroy()
        {
            audioMixer.SetFloat(volumeParam, startingVolume);
        }

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            audioMixer = audioSource.outputAudioMixerGroup.audioMixer;
            audioMixer.GetFloat(volumeParam, out  startingVolume);
        }


        private void Start()
        {
            if (playOnStart) 
            {
                if(loop)
                {
                    PlayOnInterval();
                }
                else
                {
                    PlayForDuration();
                }
            }
        }

        private void Update()
        {
            if(audioSource == null)
            {
                Destroy(gameObject);
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
            if(!currentlyPlaying)
            {
                currentlyPlaying = true;
                continuePlaying = true;
                StartCoroutine(EffectOnInterval());
            }
        }

        public void stopPlayOnInterval()
        {
            continuePlaying = false;
            currentlyPlaying = false;
        }

        public void stopSound()
        {
            StartCoroutine(StopAfterDelay(0));
        }

        IEnumerator StopAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            StartCoroutine(FadeAudioMixer.StartFade(audioMixer, volumeParam, fadeOutDuration, 0.00f));
        }

        IEnumerator EffectOnInterval()
        {
            while(continuePlaying)
            {
                audioSource.PlayOneShot(GetRandomClip(audioClips));
                yield return new WaitForSeconds(interval);
            }
        }

        AudioClip GetRandomClip(List<AudioClip> clips) => clips[UnityEngine.Random.Range(0, clips.Count)];
    }
}