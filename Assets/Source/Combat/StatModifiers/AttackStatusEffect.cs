using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.VisualScripting;
using UnityEngine;

namespace ElementalEngagement.Combat
{
    /// <summary>
    /// Used to buff/debuff attacks in an area.
    /// </summary>
    public class AttackStatusEffect : StatusEffect
    {
        [Tooltip("The amount to add to the damage of any attacks in the area as a percent of its current value.")]
        [Min(0.00001f)]
        [SerializeField] private float damageMultiplier = 1;

        [Tooltip("The amount to add to the knockback amount of any attacks in the area as a percent of its current value.")]
        [Min(0.00001f)]
        [SerializeField] private float knockbackMultiplier = 1;

        [Tooltip("The amount to add to the attack interval of any attacks in the area as a percent of its current value.")]
        [Min(0.00001f)]
        [SerializeField] private float attackIntervalMultiplier = 1;

        private Collider[] collidersToEffect;

        /// <summary>
        /// Checks all objects in the area of effect and applies proper changes to each one
        /// </summary>
        private void Start()
        {
            collidersToEffect = Physics.OverlapSphere(area.transform.position, area.radius);
            foreach (Collider collider in collidersToEffect)
            {
                Attack attack = collider.GetComponent<Attack>();

                if (attack == null)
                    continue;
                if (!CanModify(collider))
                    continue;

                attack.damage.amount *= damageMultiplier;
                attack.knockback.amount *= knockbackMultiplier;
                attack.attackInterval *= attackIntervalMultiplier;
            }
        }

        /// <summary>
        /// Same as start, but restores values to original
        /// </summary>
        public void OnDestroy()
        {
            foreach (Collider collider in collidersToEffect)
            {
                Attack attack = collider.GetComponent<Attack>();

                if (attack == null)
                    continue;
                if (!CanModify(collider))
                    continue;

                attack.damage.amount /= damageMultiplier;
                attack.knockback.amount /= knockbackMultiplier;
                attack.attackInterval /= attackIntervalMultiplier;
            }
        }
    }
}