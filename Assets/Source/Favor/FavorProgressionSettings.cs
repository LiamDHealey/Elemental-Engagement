using ElementalEngagement.Combat;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ReadOnlyDictionary<MinorGod, GodProgressionSettings> godProgressionSettings { get { throw new System.NotImplementedException(); } }





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
            public ReadOnlyDictionary<string, GodProgressionSettings> spawnerTypes { get { throw new System.NotImplementedException(); } }







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
            private class SpawnerType
            {
                [Tooltip("The name used by spawner to pick these settings")]
                [SerializeField] public string typeName = "Default";

                [Tooltip("How the spawn interval (in seconds/unit) changes as the favor increases. Favor is normalized 0-1")]
                [SerializeField] public AnimationCurve favorToSpawnInterval;

                [Tooltip("What thing to spawn depending on the favor.")]
                [SerializeField] public List<Unlock<GameObject>> spawnConditions;
            }
        }
    }
}