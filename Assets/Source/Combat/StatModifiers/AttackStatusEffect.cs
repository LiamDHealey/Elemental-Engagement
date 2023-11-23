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

        private void Start()
        {
            area.overlappingColliders.CopyTo(collidersToEffect, area.overlappingColliders.Count);
            foreach (Collider collider in collidersToEffect)
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
        }

        public void OnDestroy()
        {
            foreach (Collider collider in collidersToEffect)
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
}