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
        [Tooltip("The name used to determine this spawner's settings. Setting are stored in the favor progression settings.")]
        [SerializeField] private string spawnerType = "Default";

        [Tooltip("The location to spawn the units at.")]
        [SerializeField] private Transform spawnLocation;

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
            spawnAllegiance = GetComponent<Player.Allegiance>();
            timeSinceLastSpawn = 0;
        }

        /// <summary>
        /// Calculates the time since the last object was spawned and spawns the object
        /// </summary>
        public void Update()
        {
            float spawnInterval;
            if(spawnRate() != 0.0)
            {
                spawnInterval = 1 / spawnRate();
            }
            else
            {
                return; 
            }

            timeSinceLastSpawn += Time.deltaTime;
            if(timeSinceLastSpawn >= spawnInterval)
            {
                GameObject spawnedObject = Instantiate(objectToSpawn());

                NavMesh.SamplePosition(spawnLocation.position, out NavMeshHit navMeshHit, 50f, 1);
                spawnedObject.transform.position = navMeshHit.position;

                spawnedObject.GetComponent<Allegiance>().faction = spawnAllegiance.faction;
                onSpawned?.Invoke();

                timeSinceLastSpawn = 0;
            }
        }

        private float spawnRate()
        {
                   //Accessing the GodProgressionSettings of the FavorManager
            return FavorManager.progressionSettings[spawnAllegiance.god]
                   //Progressing from GodProgressionSettings to the Animation Curve
                   .spawnerTypes[spawnerType].favorToSpawnRate
                   //Evaluating the Animation Curve at the current Faction value
                   .Evaluate(FavorManager.factionToFavor[new System.Tuple<Player.Faction, MinorGod> (spawnAllegiance.faction, spawnAllegiance.god)]);
        }

        private GameObject objectToSpawn()
        {
            //Accessing the GodProgressionSettings of the FavorManager
            return FavorManager.progressionSettings[spawnAllegiance.god]
                   //Progressing from GodProgressionSettings to the Prefab to Spawn
                   .spawnerTypes[spawnerType].prefabToSpawn;
        }
    }
}