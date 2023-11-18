using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace ElementalEngagement.Favor
{
    /// <summary>
    /// Spawns in new units aligned with it based on favor the minor god this is aligned with.
    /// </summary>
    [RequireComponent(typeof(Player.Allegiance))]
    public class Spawner : MonoBehaviour
    {
        [Tooltip("The thing to spawn")]
        [SerializeField] private GameObject objectToSpawn;

        [Tooltip("The location to spawn the units at.")]
        [SerializeField] public Transform spawnLocation;

        [Tooltip("The time between each spawn before any multipliers are applied.")]
        [SerializeField] public float baseInterval = 5f;

        [Tooltip("The location to spawn the units at.")]
        public UnityEvent onSpawned;

        //The Allegiance tied to this spawner
        Allegiance spawnAllegiance;

        //The time since the last object was spawned
        float timeSinceLastSpawn;

        /// <summary>
        /// Prepares to spawn objects at the start of the game
        /// </summary>
        public void Awake()
        {
            spawnAllegiance = GetComponent<Allegiance>();
            timeSinceLastSpawn = 0;
        }

        /// <summary>
        /// Calculates the time since the last object was spawned and spawns the object
        /// </summary>
        public void Update()
        {
            float spawnInterval = baseInterval / SpawnrateProvider.GetSpawnrateMultiplier(spawnAllegiance);
            
            timeSinceLastSpawn += Time.deltaTime;
            if(timeSinceLastSpawn >= spawnInterval)
            {
                GameObject spawnedObject = Instantiate(objectToSpawn);

                Vector2 noise = Random.insideUnitCircle;
                spawnedObject.transform.position = spawnLocation.position + new Vector3(noise.x, 0, noise.y);
                spawnedObject.GetComponent<Allegiance>().faction = spawnAllegiance.faction;
                onSpawned?.Invoke();

                timeSinceLastSpawn = 0;
            }
        }
    }
}