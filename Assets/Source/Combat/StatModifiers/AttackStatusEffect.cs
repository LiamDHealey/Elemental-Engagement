using ElementalEngagement.Player;
using JetBrains.Annotations;
using System;
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

        [Tooltip("Whether this status effect changes whether attacks wait their cooldown before the first attack")]
        [SerializeField] private bool changeAttackPattern = false;

        [Tooltip("If Previous setting is true, what to change the pattern to")]
        [SerializeField] private bool waitForDamage = false;

        private List<Attack> attacks;
        private Collider[] collidersToEffect;

        /// <summary>
        /// Checks all objects in the area of effect and applies proper changes to each one
        /// </summary>
        private void Start()
        {

            attacks = new List<Attack>();

            if (singleTarget)
            {
                if (gameObject.transform.parent == null) { return; }
                Attack attack = gameObject.transform.parent.GetComponent<Attack>();

                attacks.Add(attack);

                foreach (var test in gameObject.transform.parent.GetComponentsInChildren<Attack>())
                {
                    attacks.Add(test);
                }

                foreach (Attack tempAttack in attacks)
                {
                    if (tempAttack == null)
                        continue;
                    tempAttack.damage.amount *= damageMultiplier;
                    tempAttack.knockback.amount *= knockbackMultiplier;
                    tempAttack.SetAttackInterval(tempAttack.attackInterval * attackIntervalMultiplier, true);
                }
                return;
            }

            collidersToEffect = Physics.OverlapSphere(area.transform.position, area.radius);
            foreach (Collider collider in collidersToEffect)
            {
                Attack attack = collider.GetComponent<Attack>();

                if (attack == null)
                    continue;
                if (!CanModify(collider))
                    continue;

                attacks.Add(attack);

                foreach (var test in collider.GetComponentsInChildren<Attack>())
                {
                    attacks.Add(test);
                }
            }


            foreach (Attack tempAttack in attacks)
            {
                tempAttack.damage.amount *= damageMultiplier;
                tempAttack.knockback.amount *= knockbackMultiplier;
                tempAttack.SetAttackInterval(tempAttack.attackInterval * attackIntervalMultiplier, true);
            }
        }

        /// <summary>
        /// Same as start, but restores values to original
        /// </summary>
        public void OnDestroy()
        {

            if (singleTarget)
            {
                if (gameObject.transform.parent == null) { return; }

                foreach (Attack tempAttack in attacks)
                {
                    if (tempAttack == null)
                        continue;

                    tempAttack.damage.amount /= damageMultiplier;
                    tempAttack.knockback.amount /= knockbackMultiplier;
                    tempAttack.SetAttackInterval(tempAttack.attackInterval / attackIntervalMultiplier, false);
                }
                return;
            }

            foreach (Attack tempAttack in attacks)
            {
                if (tempAttack == null)
                    continue;

                if (changeAttackPattern)
                {
                    tempAttack.waitBeforeDamage = !waitForDamage;
                }
                tempAttack.damage.amount /= damageMultiplier;
                tempAttack.knockback.amount /= knockbackMultiplier;
                tempAttack.SetAttackInterval(tempAttack.attackInterval / attackIntervalMultiplier, false);
            }
        }
    }
}