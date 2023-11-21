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
        [SerializeField] List<FactionToOverrides> factionsToOverrides = new List<FactionToOverrides>();
        HashSet<Faction> claimedFactions = new HashSet<Faction>();

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
    }
}
