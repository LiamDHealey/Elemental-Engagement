using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ElementalEngagement.Combat
{
    /// <summary>
    /// Deals damage to things in an area.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class AOEAttack : Attack
    {
        [Tooltip("The collider in which the target must be within to take damage.")]
        [SerializeField] private BindableCollider attackRange;

        [Tooltip("The maximum number of things this can hit at once.")] [Min(1)]
        [SerializeField] private int maxTargets = 1;

        /// <summary>
        /// Binds events
        /// </summary>
        private void Start()
        {
            attackRange.onTriggerEnter.AddListener(TriggerEntered);
        }

        /// <summary>
        /// Starts damaging other if not aligned and in range.
        /// </summary>
        /// <param name="other"></param>
        private void TriggerEntered(Collider other)
        {
            Health health = other.GetComponent<Health>();
            KnockbackReceiver knockbackReceiver = other.GetComponent<KnockbackReceiver>();
            Allegiance otherAllegiance = other.GetComponent<Allegiance>();

            if (health == null && knockbackReceiver == null)
                return;

            // If the target is aligned with this attack
            if (allegiance != null && otherAllegiance != null &&
                allegiance.faction == otherAllegiance.faction) { return; }

            StartCoroutine(DamageOverTime());

            /// <summary>
            /// Deals damage at the appropriate interval.
            /// </summary>
            IEnumerator DamageOverTime()
            {
                if (waitBeforeDamage)
                    yield return new WaitForSeconds(attackInterval);

                while (attackRange.overlappingColliders.Contains(other))
                {
                    if (attackRange.overlappingColliders.Count <= maxTargets || attackRange.overlappingColliders.IndexOf(other) < maxTargets)
                    {
                        onAttackStart?.Invoke();

                        if (damageDelay > 0)
                            yield return new WaitForSeconds(damageDelay);

                        health.TakeDamage(damage);
                        knockbackReceiver.ReceiveKnockback(knockback);
                        onAttackDamage?.Invoke();
                    }

                    yield return new WaitForSeconds(attackInterval - damageDelay);
                }
            }
        }
    }
}