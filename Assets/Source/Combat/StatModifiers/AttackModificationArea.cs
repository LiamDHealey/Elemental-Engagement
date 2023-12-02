using ElementalEngagement.Player;
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
        [Min(0.00001f)]
        [SerializeField] private float damageMultiplier = 1;

        [Tooltip("The amount to add to the knockback amount of any attacks in the area as a percent of its current value.")]
        [Min(0.00001f)]
        [SerializeField] private float knockbackMultiplier = 1;

        [Tooltip("The amount to add to the attack interval of any attacks in the area as a percent of its current value.")]
        [Min(0.00001f)]
        [SerializeField] private float attackIntervalMultiplier = 1;


        private void Start()
        {
            area.onTriggerEnter.AddListener(OnTriggerEntered);
            area.onTriggerExit.AddListener(OnTriggerExited);
        }

        public void OnDestroy()
        {
            foreach(Collider collider in area.overlappingColliders)
            {
                if (collider != null)
                    OnTriggerExited(collider);
            }
        }

        /// <summary>
        /// Activates when an object enters the object's collider area.
        /// If the object is a unit, then apply the damage, knockback, and attack interval upgrades
        /// to that unit.
        /// </summary>
        /// <param name="collider"></param>
        void OnTriggerEntered(Collider collider)
        {
            Attack attack = collider.GetComponent<Attack>();

            if (attack == null)
                return;
            if (!CanModify(collider))
                return;

            attack.damage.amount *= damageMultiplier;
            attack.knockback.amount *= knockbackMultiplier;
            attack.attackInterval *= attackIntervalMultiplier;
        }

        /// <summary>
        /// Activates when an object exits the object's collider area.
        /// If the object is a unit, then apply the damage, knockback, and attack interval downgrades
        /// to that unit.
        /// </summary>
        /// <param name="collider"></param>
        void OnTriggerExited(Collider collider)
        {
            Attack attack = collider.GetComponent<Attack>();

            if (attack == null)
                return;
            if (!CanModify(collider))
                return;

            attack.damage.amount /= damageMultiplier;
            attack.knockback.amount /= knockbackMultiplier;
            attack.attackInterval /= attackIntervalMultiplier;
        }
    }
}
