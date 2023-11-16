using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace ElementalEngagement.Favor
{
    [RequireComponent(typeof(Allegiance))]
    public class SpawnrateProvider : MonoBehaviour
    {
        [Tooltip("The unit to provide a spawnrate boost to")]
        public float spawnRateMultiplier = 1.25f;

        private static List<SpawnrateProvider> providers = new List<SpawnrateProvider>();

        private Allegiance allegiance;

        public static float GetSpawnrateMultiplier(Allegiance allegiance)
        {
            float multiplier = 1f;
            foreach (SpawnrateProvider provider in providers)
            {
                if (!allegiance.CheckBothAllegiance(provider.allegiance))
                    continue;
                multiplier *= provider.spawnRateMultiplier;
            }
            return multiplier;
        }

        // Start is called before the first frame update
        void Start()
        {
            allegiance = GetComponent<Allegiance>();
            providers.Add(this);
        }

        // Update is called once per frame
        void OnDestroy()
        {
            providers.Remove(this);
        }
    }
}