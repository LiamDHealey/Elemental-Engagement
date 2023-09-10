using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElementalEngagement.Combat
{
    [CreateAssetMenu]
    public class Ability : ScriptableObject
    {
        [Tooltip("The icon used for this ability on the UI.")]
        public Sprite icon;

        [Tooltip("The cooldown of this ability in seconds.")] [Min(0)]
        public float cooldown = 10f;

        [Tooltip("The prefab to spawn under the cursor when this is played.")]
        public GameObject prefabToSpawn;

        [Tooltip("Whether or not the spawned prefab should have it's player allegiance (if it has one) to the same as the player who spawned it.")]
        public bool inheritPlayerAllegiance;
    }
}