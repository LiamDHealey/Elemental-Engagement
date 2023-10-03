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
    public class MusicManager : MonoBehaviour
    {
        [Tooltip("The time it takes for a track to fade out after another starts.")] [Min(0f)]
        [SerializeField] private float fadeOutDuration = 0.5f;

        [Header("Menu Music")]
        [Tooltip("The exact name of the main menu scene")]
        [SerializeField] private string mainMenuSceneName;

        [Tooltip("The sound that will be played right as the menu is opened.")]
        [SerializeField] private List<AudioClip> menuStartClips = new List<AudioClip>();

        [Tooltip("The music that will play on the menu")]
        [SerializeField] private List<AudioClip> menuMusic = new List<AudioClip>();



        [Header("Low Intensity")]

        [Tooltip("The sound that will be played right as low intensity music starts")]
        [SerializeField] private List<AudioClip> lowIntensityStartClips = new List<AudioClip>();

        [Tooltip("The music that will play by default")]
        [SerializeField] private List<AudioClip> lowIntensityMusic = new List<AudioClip>();



        [Header("Medium Intensity")]
        [Tooltip("The favor that needs to be reached by one player for one god for the medium track to start.")] [Range(0f, 1f)]
        [SerializeField] private float favorThreshold = 0.5f;

        [Tooltip("The sound that will be played right as medium intensity music starts")]
        [SerializeField] private List<AudioClip> mediumIntensityStartClips = new List<AudioClip>();

        [Tooltip("The music that will play when a player's favor reaches the threshold")]
        [SerializeField] private List<AudioClip> mediumIntensityMusic = new List<AudioClip>();



        [Header("High Intensity")]

        [Tooltip("The sound that will be played right as high intensity music starts")]
        [SerializeField] private List<AudioClip> highIntensityStartClips = new List<AudioClip>();

        [Tooltip("The music that will play once the human army is spawned.")]
        [SerializeField] private List<AudioClip> highIntensityMusic = new List<AudioClip>();

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
            instance.StartCoroutine(PlayMusic(State.Menu, instance.menuStartClips, instance.menuMusic));
        }

        public static void StartLowIntensityMusic()
        {
            instance.state = State.LowIntensity;
            instance.StartCoroutine(PlayMusic(State.LowIntensity, instance.lowIntensityStartClips, instance.lowIntensityMusic));
        }

        public static void StartMediumIntensityMusic()
        {
            instance.state = State.MediumIntensity;
            instance.StartCoroutine(PlayMusic(State.MediumIntensity, instance.mediumIntensityStartClips, instance.mediumIntensityMusic));
        }

        public static void StartHighIntensityMusic()
        {
            instance.state = State.HighIntensity;
            instance.StartCoroutine(PlayMusic(State.HighIntensity, instance.highIntensityStartClips, instance.highIntensityMusic));

        }


        private static IEnumerator PlayMusic(State state, List<AudioClip> startClips, List<AudioClip> music)
        {
            AudioSource audioSource = Instantiate(MusicManager.audioSource);

            // Play start clip
            if (startClips.Count > 0)
            {
                audioSource.PlayOneShot(GetRandomClip(startClips));
            }

            // Play music
            while (instance.state == state)
            {
                yield return null;
                if (audioSource.isPlaying)
                    continue;

                audioSource.PlayOneShot(GetRandomClip(music));
            }

            // Fade out music
            float fadeSpeed = audioSource.volume / instance.fadeOutDuration;
            while (audioSource.volume > 0)
            {
                audioSource.volume -= fadeSpeed * Time.deltaTime;
            }
            Destroy(audioSource);




            AudioClip GetRandomClip(List<AudioClip> clips) => startClips[UnityEngine.Random.Range(0, startClips.Count)];
        }
        private enum State { Menu, LowIntensity, MediumIntensity, HighIntensity }
    }
}
