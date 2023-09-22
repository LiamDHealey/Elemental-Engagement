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
        [Tooltip("The maximum number of things this can hit at once.")] [Min(1)]
        [SerializeField] private int maxTargets = 1;

        // The number of targets this is currently able to damage.
        List<Collider> targets = new List<Collider>();


        /// <summary>
        /// Starts damaging other if not aligned and in range.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            Health health = other.GetComponent<Health>();
            KnockbackReceiver knockbackReceiver = other.GetComponent<KnockbackReceiver>();
            Allegiance otherAllegiance = other.GetComponent<Allegiance>();

            if (health == null && knockbackReceiver == null)
                return;

            // If the target is aligned with this attack
            if (allegiance != null && otherAllegiance != null &&
                allegiance.faction == otherAllegiance.faction) { return; }

            targets.Add(other);
            StartCoroutine(DamageOverTime());

            /// <summary>
            /// Deals damage at the appropriate interval.
            /// </summary>
            IEnumerator DamageOverTime()
            {
                if (waitBeforeDamage)
                    yield return new WaitForSeconds(attackInterval);

                while (targets.Contains(other))
                {
                    if (targets.Count <= maxTargets || targets.IndexOf(other) < maxTargets)
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

        /// <summary>
        /// Remove a target so it cannot be damaged.
        /// </summary>
        /// <param name="other"> The target to remove. </param>
        private void OnTriggerExit(Collider other)
        {
            targets.Remove(other);
        }
    }
}