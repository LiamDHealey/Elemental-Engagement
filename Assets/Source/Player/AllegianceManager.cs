using ElementalEngagement.Favor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace ElementalEngagement.Player
{
    /// <summary>
    /// Aligns an object with a player.
    /// </summary>
    public class AllegianceManager : MonoBehaviour
    {
        public static UnityEvent onGameStarted = new UnityEvent();

        [SerializeField] List<FactionToOverrides> factionsToOverrides = new List<FactionToOverrides>();
        HashSet<Faction> claimedFactions = new HashSet<Faction>();

        private float _timeUntilGameStart = 5;
        public static float timeUntilGameStart => instance._timeUntilGameStart;
        private bool _gameStarted;
        public static bool gameStarted => instance._gameStarted;
        private static AllegianceManager instance;
        public void OnPlayerJoined(PlayerInput input)
        {
            foreach (Faction faction in Enum.GetValues(typeof(Faction)))
            {
                if (faction == Faction.Unaligned)
                    continue;
                if (!claimedFactions.Contains(faction))
                {
                    claimedFactions.Add(faction);
                    input.GetComponent<Allegiance>().faction = faction;

                    FactionToOverrides overrides = factionsToOverrides.First(o => o.faction == faction);
                    input.transform.position = overrides.spawnTransform.position;

                    GameObject Hud = Instantiate(overrides.hud);
                    Hud.transform.SetParent(input.transform);

                    Camera camera = input.GetComponent<Camera>();
                    camera.cullingMask -= 1 << LayerMask.NameToLayer(faction == Faction.PlayerOne ? "HiddenFromP1" : "HiddenFromP2");
                    Hud.GetComponent<Canvas>().worldCamera = camera;

                    RectTransform childRect = ((RectTransform)Hud.transform.GetChild(0));
                    childRect.anchorMin = camera.rect.min;
                    childRect.anchorMax = camera.rect.max;
                    childRect.offsetMin = Vector2.zero;
                    childRect.offsetMax = Vector2.zero;

                    return;
                }
            }
            Debug.LogError("Tried to join a player when all the factions have been taken.");
        }

        public void OnPlayerLeft(PlayerInput input)
        {
            claimedFactions.Remove(input.GetComponent<Allegiance>().faction);
        }

        [System.Serializable]
        class FactionToOverrides
        {
            public Faction faction;
            public Transform spawnTransform;
            public GameObject hud;
        }
        private void Start()
        {
            instance = this;
            Time.timeScale = 0;
        }

        private void Update()
        {
            if (!gameStarted && claimedFactions.Count == 2)
            {
                _timeUntilGameStart -= Time.unscaledDeltaTime;
                if (timeUntilGameStart <= 0)
                {
                    StartGame();
                }
            }
        }

        private void StartGame()
        {
            _gameStarted = true;
            Time.timeScale = 1;
            onGameStarted?.Invoke();
        }
    }
}
