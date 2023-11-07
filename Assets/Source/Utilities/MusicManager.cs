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
    public class MusicManager : MonoBehaviour
    {
        [Tooltip("The time it takes for a track to fade out after another starts.")] [Min(0f)]
        [SerializeField] private float fadeOutDuration = 0.5f;

        [Header("Menu Music")]
        [Tooltip("The exact name of the main menu scene")]
        [SerializeField] private string mainMenuSceneName;

        [Tooltip("The music that will play on the menu")]
        [SerializeField] private List<MusicTrack> menuMusic = new List<MusicTrack>();



        [Header("Low Intensity")]

        [Tooltip("The music that will play by default")]
        [SerializeField] private List<MusicTrack> lowIntensityMusic = new List<MusicTrack>();



        [Header("Medium Intensity")]
        [Tooltip("The favor that needs to be reached by one player for one god for the medium track to start.")] [Range(0f, 1f)]
        [SerializeField] private float favorThreshold = 0.5f;

        [Tooltip("The music that will play when a player's favor reaches the threshold")]
        [SerializeField] private List<MusicTrack> mediumIntensityMusic = new List<MusicTrack>();



        [Header("High Intensity")]

        [Tooltip("The music that will play once the human army is spawned.")]
        [SerializeField] private List<MusicTrack> highIntensityMusic = new List<MusicTrack>();

        // Tracks the state of this machine
        private State state = State.Menu;

        // Tracks the instance of this.
        private static MusicManager instance = null;

        // Track the audio source of the music
        private static AudioSource audioSource = null;


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

        private void Start()
        {
            // Reset music on scene change.
            SceneManager.activeSceneChanged += static delegate { ResetMusic(); } ;
            // Detect medium intensity start.
            FavorManager.onFavorChanged.AddListener(static delegate
            {
                if (instance.state >= State.MediumIntensity)
                    return;

                foreach (KeyValuePair<(Player.Faction, MinorGod), float> factionToFavor in FavorManager.factionToFavor)
                {
                    if (factionToFavor.Value > instance.favorThreshold)
                    {
                        StartMediumIntensityMusic();
                        return;
                    }
                }
            });
            // Detect high intensity start.
            FavorManager.onFavorChanged.AddListener(static delegate
            {
                if (instance.state >= State.HighIntensity)
                    return;

                IEnumerator factionEnumerator = Enum.GetValues(typeof(Faction)).GetEnumerator();
                factionEnumerator.MoveNext();// Skip unaligned
                while (factionEnumerator.MoveNext())
                {
                    if (FavorManager.factionToFavor[((Faction)factionEnumerator.Current, MinorGod.Human)] >= 1)
                    {
                        StartHighIntensityMusic();
                        return;
                    }
                }
            });

            // Play music
            ResetMusic();
        }

        public static void ResetMusic()
        {
            foreach (Transform child in instance.transform)
            {
                Destroy(child.gameObject);
            }

            if (SceneManager.GetActiveScene().name == instance.mainMenuSceneName)
            {
                StartMenuMusic();
            }
            else
            {
                StartLowIntensityMusic();
            }
        }

        public static void StartMenuMusic()
        {
            instance.state = State.Menu;
            instance.StartCoroutine(PlayMusic(State.Menu, instance.menuMusic));
        }

        public static void StartLowIntensityMusic()
        {
            instance.state = State.LowIntensity;
            instance.StartCoroutine(PlayMusic(State.LowIntensity, instance.lowIntensityMusic));
        }

        public static void StartMediumIntensityMusic()
        {
            instance.state = State.MediumIntensity;
            instance.StartCoroutine(PlayMusic(State.MediumIntensity, instance.mediumIntensityMusic));
        }

        public static void StartHighIntensityMusic()
        {
            instance.state = State.HighIntensity;
            instance.StartCoroutine(PlayMusic(State.HighIntensity, instance.highIntensityMusic));
        }


        private static IEnumerator PlayMusic(State state, List<MusicTrack> music)
        {
            int musicIndex = UnityEngine.Random.Range(0, music.Count);

            GameObject container = new GameObject($"MusicPlayer_{state}_Track{musicIndex}");
            container.transform.parent = instance.transform;
            AudioSource audioSource = container.AddComponent<AudioSource>();
            yield return null;


            // Play start clip
            if (music[musicIndex].introClip != null)
            {
                audioSource.PlayOneShot(music[musicIndex].introClip);
                while (audioSource.isPlaying && instance.state == state)
                {
                    yield return null;
                }
            }

            if(instance.state == state)
            {
                // Play music
                audioSource.clip = music[musicIndex].mainClip;
                audioSource.loop = true;
                audioSource.Play();
                while (instance.state == state)
                {
                    yield return null;
                }
            }

            // Fade out music
            float fadeSpeed = audioSource.volume / instance.fadeOutDuration;
            while (audioSource.volume > 0)
            {
                audioSource.volume -= fadeSpeed * Time.deltaTime;
                yield return null;
            }
            Destroy(audioSource.gameObject);
        }
        private enum State { Menu, LowIntensity, MediumIntensity, HighIntensity }


        [System.Serializable]
        private class MusicTrack
        {
            public AudioClip introClip;
            public AudioClip mainClip;
        }
    }
}
