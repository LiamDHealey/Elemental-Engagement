using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElementalEngagement.Combat
{
    /// <summary>
    /// Used to buff/debuff attacks in an area.
    /// </summary>
    public class AttackModificationArea : StatModificationArea
    {
        [Tooltip("The amount to add to the damage of any attacks in the area as a percent of its current value.")]
        [SerializeField] private float deltaDamage;

        [Tooltip("The amount to add to the knockback amount of any attacks in the area as a percent of its current value.")]
        [SerializeField] private float deltaKnockback;

        [Tooltip("The amount to add to the attack interval of any attacks in the area as a percent of its current value.")]
        [SerializeField] private float deltaAttackInterval;

        /// <summary>
        /// Activates when an object enters the object's collider area.
        /// If the object is a unit, then apply the damage, knockback, and attack interval upgrades
        /// to that unit.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            
        }

    }
}