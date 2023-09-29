using ElementalEngagement.Combat;
using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace ElementalEngagement.Player
{
    public class VictoryDetector : MonoBehaviour
    {
        [Tooltip("The players and the structures they need to keep alive to stay in the game.")]
        [SerializeField] private List<PlayerToVitalStructures> playersToVitalStructures;

        [Tooltip("Called when a player has lost.")]
        public UnityEvent<Faction> onPlayerLost;

        // The factions map to their loose conditions.
        public ReadOnlyDictionary<Faction, ReadOnlyCollection<Health>> factionToVitalStructures;

        private void Start()
        {
            factionToVitalStructures = new ReadOnlyDictionary<Faction, ReadOnlyCollection<Health>>(
                playersToVitalStructures.ToDictionary(ptvs => ptvs.faction, ptvs => ptvs.vitalStructures.AsReadOnly()));

            foreach (PlayerToVitalStructures playerToVitalStructures in playersToVitalStructures)
            {
                foreach (Health vitalStructure in playerToVitalStructures.vitalStructures)
                {
                    vitalStructure.onKilled.AddListener(CheckForLosses);
                    void CheckForLosses()
                    {
                        // See if any other vital structure for this faction are still alive
                        foreach (Health otherVitalStructure in playerToVitalStructures.vitalStructures)
                        {
                            if (otherVitalStructure != null)
                                return;
                        }

                        onPlayerLost?.Invoke(playerToVitalStructures.faction);
                    }
                }
            }
        }

        /// <summary>
        /// Class for mapping factions to vital structures.
        /// </summary>
        private class PlayerToVitalStructures
        {
            [Tooltip("The faction with who's vital structures are listed.")]
            public Faction faction;

            [Tooltip("The structures that the faction must not let be killed.")]
            public List<Health> vitalStructures;
        }
    }
}