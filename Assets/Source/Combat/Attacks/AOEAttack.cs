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
    public class AOEAttack : Attack
    {
        [Tooltip("The collider in which the target must be within to take damage.")]
        [SerializeField] private BindableCollider attackRange;

        [Tooltip("The maximum number of things this can hit at once.")] [Min(1)]
        [SerializeField] private int maxTargets = 1;

        // Contains a list of all valid things for this aoe to hit.
        private List<Collider> validTargets = new List<Collider>();

        /// <summary>
        /// Binds events
        /// </summary>
        private void Awake()
        {
            attackRange.onTriggerEnter.AddListener(TriggerEntered);
            attackRange.onTriggerExit.AddListener( collider => validTargets.Remove(collider));
        }

        private void OnEnable()
        {
            foreach (Collider target in validTargets)
            {
                TriggerEntered(target);
            }
        }
        private void OnDisable()
        {
            StopAllCoroutines();
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

            validTargets.Add(other);

            StartCoroutine(DamageOverTime());

            /// <summary>
            /// Deals damage at the appropriate interval.
            /// </summary>
            IEnumerator DamageOverTime()
            {
                if (waitBeforeDamage)
                    yield return new WaitForSeconds(attackInterval);

                while (validTargets.Contains(other))
                {
                    if (other == null)
                    {
                        validTargets.Remove(other);
                        yield break;
                    }

                    if (validTargets.Count <= maxTargets || validTargets.IndexOf(other) < maxTargets)
                    {
                        onAttackStart?.Invoke();

                        if (damageDelay > 0)
                            yield return new WaitForSeconds(damageDelay);

                        health?.TakeDamage(damage);
                        knockbackReceiver?.ReceiveKnockback(knockback);
                        onAttackDamage?.Invoke();
                    }

                    yield return new WaitForSeconds(attackInterval - damageDelay);
                }
            }
        }
    }
}