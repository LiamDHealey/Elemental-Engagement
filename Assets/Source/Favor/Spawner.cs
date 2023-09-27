using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElementalEngagement.Favor
{
    /// <summary>
    /// Spawns in new units aligned with it based on favor the minor god this is aligned with.
    /// </summary>
    [RequireComponent(typeof(Player.Allegiance))]
    public class Spawner : MonoBehaviour
    {
        [Tooltip("The name used to determine this spawner's settings. Setting are stored in the favor progression settings.")]
        [SerializeField] private string spawnerType = "Default";

        [Tooltip("The location to spawn the units at.")]
        [SerializeField] private Transform spawnLocation;
    }
}