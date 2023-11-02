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

            [Tooltip("All the ways this god's spawners can spawn units.")]
            [SerializeField] private List<SpawnerType> _spawnerTypes;
            public ReadOnlyDictionary<string, SpawnerType> spawnerTypes { get => new ReadOnlyDictionary<string, SpawnerType>(_spawnerTypes.ToDictionary(s => s.typeName)); }



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

            /// <summary>
            /// Stores info related to a single spawner type.
            /// </summary>
            [System.Serializable]
            public class SpawnerType
            {
                [Tooltip("The name used by spawner to pick these settings")]
                [SerializeField] public string typeName = "Default";

                [Tooltip("How the spawn rate (in units/second) changes as the favor increases. Favor is normalized 0-1.")]
                [SerializeField] public AnimationCurve favorToSpawnRate;

                [Tooltip("The prefab to instantiate when this spawns a thing.")]
                [SerializeField] public GameObject prefabToSpawn;
            }
        }
    }
}