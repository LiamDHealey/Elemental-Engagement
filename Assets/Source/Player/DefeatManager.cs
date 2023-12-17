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
    public class DefeatManager : MonoBehaviour
    {
        [Tooltip("The players and the structures they need to keep alive to stay in the game.")]
        [SerializeField] private List<PlayerToVitalStructures> playersToVitalStructures;

        [Tooltip("Called when a player has lost.")]
        [SerializeField] private UnityEvent<Faction> _onPlayerLost;

        public static UnityEvent<Faction> onPlayerLost => instance?._onPlayerLost;

        // The factions map to their loose conditions.
        public static ReadOnlyDictionary<Faction, ReadOnlyCollection<Health>> factionToVitalStructures;

        // Stores a list of the currently surviving factions.
        private static List<Faction> _survivingFactions;
        public static ReadOnlyCollection<Faction> survivingFactions;

        // Tracks the singleton instance of this.
        private static DefeatManager instance;

        private void Awake() => instance = this;

        private void Start()
        {
            factionToVitalStructures = new ReadOnlyDictionary<Faction, ReadOnlyCollection<Health>>(
                playersToVitalStructures.ToDictionary(ptvs => ptvs.faction, ptvs => ptvs.vitalStructures.AsReadOnly()));

            _survivingFactions = playersToVitalStructures.Select(ptvs => ptvs.faction).ToList();
            survivingFactions = new ReadOnlyCollection<Faction>(_survivingFactions);

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
                            if (otherVitalStructure != null && otherVitalStructure.hp > 0)
                                return;
                        }

                        _survivingFactions.Remove(playerToVitalStructures.faction);
                        onPlayerLost?.Invoke(playerToVitalStructures.faction);
                    }
                }
            }
        }

        /// <summary>
        /// Class for mapping factions to vital structures.
        /// </summary>
        [System.Serializable]
        private class PlayerToVitalStructures
        {
            [Tooltip("The faction with who's vital structures are listed.")]
            public Faction faction;

            [Tooltip("The structures that the faction must not let be killed.")]
            public List<Health> vitalStructures;
        }
    }
}