using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace ElementalEngagement.Economic
{
    [CreateAssetMenu]
    public class FavorProgressionSettings : ScriptableObject
    {
        [Tooltip("How each of the gods benefits the player as their favor rises/falls.")]
        [SerializeField] private List<GodProgressionSettings> _godProgressionSettings;
        public ReadOnlyDictionary<MinorGod, GodProgressionSettings> godProgressionSettings { get { throw new System.NotImplementedException(); } }






        [System.Serializable]
        public class GodProgressionSettings
        {
            [Tooltip("The god these settings pertain to.")]
            public MinorGod god;

            [Tooltip("The favor this god starts with towards all players.")]
            public float initialFavor;

            [Tooltip("What abilities are associated with this god and when they are unlocked. Associated game object will be spawned when the favor reaches the threshold, and destroyed when it falls below the threshold.")]
            public List<Unlock> abilityUnlocks;

            [Tooltip("All the ways this god's spawners can spawn units.")]
            [SerializeField] private List<SpawnerType> _spawnerTypes;
            public ReadOnlyDictionary<string, GodProgressionSettings> spawnerTypes { get { throw new System.NotImplementedException(); } }







            /// <summary>
            /// Stores the conditions for a thing to be spawned in to spawn
            /// </summary>
            [System.Serializable]
            public class Unlock
            {
                [Tooltip("The minimum favor required to spawn this.")] [Range(0, 1)]
                public float favorThreshold;

                [Tooltip("The prefab the thing to spawn.")]
                public GameObject prefab;
            }

            /// <summary>
            /// Stores info related to a single spawner type
            /// </summary>
            [System.Serializable]
            private class SpawnerType
            {
                [Tooltip("The name used by spawner to pick these settings")]
                [SerializeField] public string typeName = "Default";

                [Tooltip("How the spawn interval (in seconds/unit) changes as the favor increases. Favor is normalized 0-1")]
                [SerializeField] public AnimationCurve favorToSpawnInterval;

                [Tooltip("What unit to spawn depending on the favor.")]
                [SerializeField] public List<Unlock> spawnConditions;
            }
        }
    }
}