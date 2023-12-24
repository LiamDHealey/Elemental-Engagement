using ElementalEngagement.Combat;
using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private int spawnCap = 100;

        [Tooltip("The thing to spawn")]
        [SerializeField] private GameObject objectToSpawn;

        [Tooltip("The location to spawn the units at.")]
        [SerializeField] public Transform spawnLocation;

        [Tooltip("The time between each spawn before any multipliers are applied.")]
        [SerializeField] public float baseInterval = 5f;

        public bool useSpawnCap = true;
        
        [Tooltip("The location to spawn the units at.")]
        public UnityEvent onSpawned;

        //The Allegiance tied to this spawner
        Allegiance spawnAllegiance;

        //The time since the last object was spawned
        float timeSinceLastSpawn;

        public static Dictionary<Faction, HashSet<GameObject>> spawnedObjects = new Dictionary<Faction, HashSet<GameObject>>()
            {
                { Faction.PlayerOne, new HashSet<GameObject>() },
                { Faction.PlayerTwo, new HashSet<GameObject>() },
            };
        public static Dictionary<Faction, int> spawnCaps = new Dictionary<Faction, int>()
            {
                { Faction.PlayerOne, 0 },
                { Faction.PlayerTwo, 0 },
            };

        /// <summary>
        /// Prepares to spawn objects at the start of the game
        /// </summary>
        public void Awake()
        {
            spawnAllegiance = GetComponent<Allegiance>();
            timeSinceLastSpawn = 0;
        }

        public void Start()
        {
            spawnedObjects[spawnAllegiance.faction] = FindObjectsOfType<Selectable>()
                .Where(x => x.allegiance.faction == spawnAllegiance.faction)
                .Select(x => x.gameObject)
                .ToHashSet();
            spawnCaps[spawnAllegiance.faction] = spawnCap;
        }

        /// <summary>
        /// Calculates the time since the last object was spawned and spawns the object
        /// </summary>
        public void Update()
        {
            float spawnInterval = baseInterval / SpawnrateProvider.GetSpawnrateMultiplier(spawnAllegiance);
            
            timeSinceLastSpawn += Time.deltaTime;
            spawnedObjects[spawnAllegiance.faction] = spawnedObjects[spawnAllegiance.faction]
                .Where(s => s != null)
                .ToHashSet();

            if (timeSinceLastSpawn >= spawnInterval)
            {
                if (useSpawnCap && spawnedObjects[spawnAllegiance.faction].Count >= spawnCaps[spawnAllegiance.faction])
                    return;

                GameObject spawnedObject = Instantiate(objectToSpawn);

                Vector2 noise = Random.insideUnitCircle;
                spawnedObject.transform.position = spawnLocation.position + new Vector3(noise.x, 0, noise.y);
                spawnedObject.GetComponent<Allegiance>().faction = spawnAllegiance.faction;
                onSpawned?.Invoke();

                if (useSpawnCap)
                {
                    spawnedObjects[spawnAllegiance.faction].Add(spawnedObject);
                    spawnedObject.GetComponent<Health>().onKilled.AddListener(() => spawnedObjects[spawnAllegiance.faction].Remove(spawnedObject));
                }

                timeSinceLastSpawn = 0;
            }
        }
    }
}