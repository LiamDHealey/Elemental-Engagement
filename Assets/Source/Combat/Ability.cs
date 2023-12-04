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
        public GameObject abilityPrefab;

        [Tooltip("The prefab to spawn under the cursor while this is being previewed.")]
        public GameObject previewPrefab;

        [Tooltip("Whether or not the spawned prefab should have it's player allegiance (if it has one) to the same as the player who spawned it.")]
        public bool inheritPlayerAllegiance;

        [Tooltip("Which ability menu this is in.")] [Range(0, 3)]
        public int menuIndex;

        [Tooltip("Which slot in the submenu this is in.")] [Range(0, 3)]
        public int submenuIndex;

        [Tooltip("Whether or not this collides with things")]
        public bool hasCollision = false;

        [Tooltip("Whether or not this can be rotated")]
        public bool canBeRotated = false;
    }
}