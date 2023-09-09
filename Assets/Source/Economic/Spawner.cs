using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElementalEngagement.Economic
{
    /// <summary>
    /// Spawns in new units aligned with it based on favor the minor god this is aligned with.
    /// </summary>
    [RequireComponent(typeof(Player.Allegiance))]
    public class Spawner : MonoBehaviour
    {
        [Tooltip("How the spawn interval (in seconds/unit) changes as the favor increases. Favor is normalized 0-1")]
        [SerializeField] private AnimationCurve favorToSpawnInterval;

        [Tooltip("What unit to spawn depending on the favor.")]
        [SerializeField] private List<UnitSpawnConditions> spawnConditions = new List<UnitSpawnConditions>() { new UnitSpawnConditions() };

        /// <summary>
        /// Stores the conditions for a unit to spawn
        /// </summary>
        [System.Serializable]
        private class UnitSpawnConditions
        {
            [Tooltip("The minimum favor required to spawn this.")] [Range(0,1)]
            public float favorThreshold;

            [Tooltip("The prefab to instantiate when spawning this unit.")]
            public Player.Allegiance unitPrefab;

            public UnitSpawnConditions(float favorThreshold = 0, Player.Allegiance unitPrefab = null)
            {
                this.favorThreshold = favorThreshold;
                this.unitPrefab = unitPrefab;
            }
        }
    }
}