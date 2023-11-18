using ElementalEngagement.Combat;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace ElementalEngagement.Favor
{
    /// <summary>
    /// Stores all of the stats of how each god's favor effects the game.
    /// </summary>
    [CreateAssetMenu]
    public class FavorProgressionSettings : ScriptableObject
    {
        [Tooltip("How each of the gods benefits the player as their favor rises/falls.")]
        [SerializeField] private List<GodProgressionSettings> _godProgressionSettings;
        public ReadOnlyDictionary<MinorGod, GodProgressionSettings> godProgressionSettings { get => new ReadOnlyDictionary<MinorGod, GodProgressionSettings>(_godProgressionSettings.ToDictionary(s => s.god)); }


        [Tooltip("The number of abilities allowed in each tier. Tier = index in the list.")]
        [SerializeField] private List<int> _abilitiesPerTier = new List<int>() { 3, 3, 2, 1 };
        public ReadOnlyCollection<int> abilitiesPerTier => _abilitiesPerTier.AsReadOnly();




        /// <summary>
        /// Stores all of the stats of how a god's favor effects the game.
        /// </summary>
        [System.Serializable]
        public class GodProgressionSettings
        {
            [Tooltip("The god these settings pertain to.")]
            public MinorGod god;

            [Tooltip("The favor this god starts with towards all players.")]
            public float initialFavor;

            [Tooltip("What abilities are associated with this god and when they are unlocked.")]
            [SerializeField] private List<AbilityUnlock> _abilityUnlocks;
            public ReadOnlyCollection<AbilityUnlock> abilityUnlocks => _abilityUnlocks.AsReadOnly();


            /// <summary>
            /// Stores the conditions an ability to be unlocked.
            /// </summary>
            [System.Serializable]
            public class AbilityUnlock
            {
                [Tooltip("The minimum favor required to spawn this.")] [Range(0, 1)]
                public float favorThreshold;

                [Tooltip("The ability tier this is in.")] [Min(0)]
                public int abilityTier;

                [Tooltip("The ability to unlock.")]
                public Ability ability;
            }
        }
    }
}