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
            public List<Unlock<Ability>> abilityUnlocks;

            [Tooltip("All the ways this god's spawners can spawn units.")]
            [SerializeField] private List<SpawnerType> _spawnerTypes;
            public ReadOnlyDictionary<string, SpawnerType> spawnerTypes { get => new ReadOnlyDictionary<string, SpawnerType>(_spawnerTypes.ToDictionary(s => s.typeName)); }







            /// <summary>
            /// Stores the conditions for a thing to be spawned.
            /// </summary>
            [System.Serializable]
            public class Unlock<T>
            {
                [Tooltip("The minimum favor required to spawn this.")] [Range(0, 1)]
                public float favorThreshold;

                [Tooltip("The thing to unlock.")]
                public T thing;
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