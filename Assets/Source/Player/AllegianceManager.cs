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
                    return;
                }
            }
            Debug.LogError("Tried to join a player when all the factions have been taken.");
        }

        public void OnPlayerLeft(PlayerInput input)
        {
            claimedFactions.Remove(input.GetComponent<Allegiance>().faction);
        }
    }
}
