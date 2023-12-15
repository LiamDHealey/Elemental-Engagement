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

        [Tooltip("The location to spawn the units at.")]
        public UnityEvent onSpawned;

        //The Allegiance tied to this spawner
        Allegiance spawnAllegiance;
        (Faction, MinorGod) allegianceKey => (spawnAllegiance.faction, spawnAllegiance.god);

        //The time since the last object was spawned
        float timeSinceLastSpawn;

        public static Dictionary<(Faction, MinorGod), HashSet<GameObject>> spawnedObjects = new Dictionary<(Faction, MinorGod), HashSet<GameObject>>()
            {
                { (Faction.PlayerOne, MinorGod.Fire), new HashSet<GameObject>() },
                { (Faction.PlayerOne, MinorGod.Earth), new HashSet<GameObject>() },
                { (Faction.PlayerOne, MinorGod.Water), new HashSet<GameObject>() },
                { (Faction.PlayerTwo, MinorGod.Fire), new HashSet<GameObject>() },
                { (Faction.PlayerTwo, MinorGod.Earth), new HashSet<GameObject>() },
                { (Faction.PlayerTwo, MinorGod.Water), new HashSet<GameObject>() },
            };
        public static Dictionary<(Faction, MinorGod), int> spawnCaps = new Dictionary<(Faction, MinorGod), int>()
            {
                { (Faction.PlayerOne, MinorGod.Fire), 0 },
                { (Faction.PlayerOne, MinorGod.Earth), 0 },
                { (Faction.PlayerOne, MinorGod.Water), 0 },
                { (Faction.PlayerTwo, MinorGod.Fire), 0 },
                { (Faction.PlayerTwo, MinorGod.Earth), 0 },
                { (Faction.PlayerTwo, MinorGod.Water), 0 },
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
            spawnedObjects[allegianceKey] = FindObjectsOfType<Selectable>()
                .Where(x => x.allegiance.faction == spawnAllegiance.faction && x.allegiance.god == spawnAllegiance.god)
                .Select(x => x.gameObject)
                .ToHashSet();
            spawnCaps[allegianceKey] = spawnCap;
        }

        /// <summary>
        /// Calculates the time since the last object was spawned and spawns the object
        /// </summary>
        public void Update()
        {
            float spawnInterval = baseInterval / SpawnrateProvider.GetSpawnrateMultiplier(spawnAllegiance);
            
            timeSinceLastSpawn += Time.deltaTime;
            spawnedObjects[allegianceKey] = spawnedObjects[allegianceKey]
                .Where(s => s != null)
                .ToHashSet();

            if (timeSinceLastSpawn >= spawnInterval)
            {
                if (spawnedObjects.Count < spawnCap)
                    return;

                GameObject spawnedObject = Instantiate(objectToSpawn);

                Vector2 noise = Random.insideUnitCircle;
                spawnedObject.transform.position = spawnLocation.position + new Vector3(noise.x, 0, noise.y);
                spawnedObject.GetComponent<Allegiance>().faction = spawnAllegiance.faction;
                onSpawned?.Invoke();

                spawnedObjects[allegianceKey].Add(spawnedObject);
                spawnedObject.GetComponent<Health>().onKilled.AddListener(() => spawnedObjects[allegianceKey].Remove(spawnedObject));

                timeSinceLastSpawn = 0;
            }
        }
    }
}