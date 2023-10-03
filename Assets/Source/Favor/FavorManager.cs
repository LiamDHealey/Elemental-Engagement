using ElementalEngagement.Combat;
using ElementalEngagement.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace ElementalEngagement.Favor
{
    /// <summary>
    /// Tracks the favor of all minor god for all player, and how that favor unlocks new things.
    /// </summary>
    public class FavorManager : MonoBehaviour
    {
        [Tooltip("Called when the favor changes for a player.")]
        public static UnityEvent<Player.Faction, MinorGod> onFavorChanged = new UnityEvent<Player.Faction, MinorGod>();


        [Tooltip("How each god's favor unlocks things.")]
        [SerializeField] private FavorProgressionSettings _progressionSettings;
        public static ReadOnlyDictionary<MinorGod, FavorProgressionSettings.GodProgressionSettings> progressionSettings { get => instance._progressionSettings.godProgressionSettings; }

        // The abilities that are currently unlocked.
        public static ReadOnlyCollection<Ability> unlockedAbilities { get => instance.tempUnlockedAbilities.AsReadOnly(); }
        // TODO: replace this with an actual implementation based off of progression settings and favor
        public List<Ability> tempUnlockedAbilities;

        // Stores the favor each god shows towards each player faction.
        [Tooltip("How much favor each god has for each player's faction.")]
        [SerializeField] private Dictionary<(Player.Faction, MinorGod), float> _factionToFavor = new Dictionary<(Player.Faction, MinorGod), float>();
        public static ReadOnlyDictionary<(Player.Faction, MinorGod), float> factionToFavor { get => new ReadOnlyDictionary<(Player.Faction, MinorGod),float>(instance._factionToFavor); }


        // Tracks the singleton instance of this.
        private static FavorManager instance;



        /// <summary>
        /// Initializes singleton.
        /// </summary>
        private void Awake()
        {
            instance = this;
            _factionToFavor = new Dictionary<(Player.Faction, MinorGod), float>()
            {
                {(Faction.PlayerOne, MinorGod.Fire), 0f},
                {(Faction.PlayerOne, MinorGod.Water), 0f},
                {(Faction.PlayerOne, MinorGod.Earth), 0f},

                {(Faction.PlayerTwo, MinorGod.Fire), 0f},
                {(Faction.PlayerTwo, MinorGod.Water), 0f},
                {(Faction.PlayerTwo, MinorGod.Earth), 0f},
            };
        }

        /// <summary>
        /// Adds an amount to the favor this god has towards a player.
        /// </summary>
        /// <param name="allegiance"> The faction to add favor for. </param>
        /// <param name="god"> The god to add favor for. </param>
        /// <param name="deltaFavor"> The amount of favor to add. </param>
        public static void ModifyFavor(Player.Faction allegiance, MinorGod god, float deltaFavor)
        {
            instance._factionToFavor[(allegiance, god)] += deltaFavor;
        }
    }
}
