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
using Unity.VisualScripting;

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
        [SerializeField]private float fadeOutDuration = 1f;

        [Tooltip("Whether or not to use the audioMixer to fade this source")]
        [SerializeField] private bool useMixer = true;

        private bool continuePlaying = false;
        private bool currentlyPlaying = false;
        private float startingMixerVolume = 0f;
        private float startingSourceVolume = 0f;

        [Tooltip("The Exact Name of the exposed volume parameter from the AudioMixer that the source is connected to")]
        [SerializeField] private string volumeParam = "godPowerVolume";

        // Track the audio source of the music
        private AudioSource audioSource = null;

        private AudioMixer audioMixer = null;

        private void OnDestroy()
        {
            audioMixer.SetFloat(volumeParam, startingMixerVolume);
        }

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            startingSourceVolume = audioSource.volume;
            audioMixer = audioSource.outputAudioMixerGroup.audioMixer;
            audioMixer.GetFloat(volumeParam, out  startingMixerVolume);
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

        /// <summary>
        /// Plays random sound and returns the length of the sound
        /// </summary>
        /// <returns>Length of Sound</returns>
        public void PlayRandomSound()
        {
            if(!useMixer)
                audioSource.volume = startingSourceVolume;

            AudioClip clipToPlay = GetRandomClip(audioClips);
            audioMixer.SetFloat(volumeParam, startingMixerVolume);
            audioSource.PlayOneShot(clipToPlay);
        }

        /// <summary>
        /// Adds this object to the queue to play an announcer sound
        /// </summary>
        public void addAnnouncementToQueue()
        {
            AnnouncerQueue.addAnnouncer(this);
        }

        /// <summary>
        /// Uses this object to play one of its loaded announcements
        /// </summary>
        /// <returns></returns>
        public float playRandomAnnouncement()
        {
            AudioClip clipToPlay = GetRandomClip(audioClips);
            float length = clipToPlay.length;
            audioSource.PlayOneShot(clipToPlay);

            return length;
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
            if(!useMixer)
            {
                StartCoroutine(FadeAudioMixer.StartSourceFade(audioSource, fadeOutDuration, 0.00f));
                return;
            }

            StartCoroutine(FadeAudioMixer.StartFade(audioMixer, volumeParam, fadeOutDuration, 0.00f));
        }

        IEnumerator StopAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            if (!useMixer)
            {
                StartCoroutine(FadeAudioMixer.StartSourceFade(audioSource, duration, 0.00f));
                yield break;
            }

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