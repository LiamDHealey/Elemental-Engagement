using ElementalEngagement.Combat;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        [SerializeField] private Dictionary<(Player.Faction, MinorGod), float> _factionToFavor;
        public ReadOnlyDictionary<(Player.Faction, MinorGod), float> factionToFavor { get => new ReadOnlyDictionary<(Player.Faction, MinorGod), float>(_factionToFavor); }


        // Tracks the singleton instance of this.
        private static FavorManager instance;



        /// <summary>
        /// Initializes singleton.
        /// </summary>
        private void Awake()
        {
            instance = this;
            if (onFavorChanged != null)
            {
                onFavorChanged = new UnityEvent<Player.Faction, MinorGod>();
            }
        }

        /// <summary>
        /// Adds an amount to the favor this god has towards a player.
        /// </summary>
        /// <param name="allegiance"> The faction to add favor for. </param>
        /// <param name="god"> The god to add favor for. </param>
        /// <param name="deltaFavor"> The amount of favor to add. </param>
        public static void ModifyFavor(Player.Faction allegiance, MinorGod god, float deltaFavor)
        {
            Dictionary<(Player.Faction, MinorGod), float> godToFavor = new Dictionary<(Player.Faction, MinorGod), float>(instance.factionToFavor);
            godToFavor[(allegiance, god)] += deltaFavor;
        }
    }
}
