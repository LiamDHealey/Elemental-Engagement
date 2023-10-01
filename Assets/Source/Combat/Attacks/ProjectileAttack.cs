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
    public class ProjectileAttack : Attack
    {
        [Tooltip("The collider in which the target must be within to take damage.")]
        [SerializeField] private BindableCollider attackRange;

        [Tooltip("The location projectile will be spawned at.")]
        [SerializeField] private Transform projectileSource;

        [Tooltip("The template to spawn as the projectile.")] 
        [SerializeField] private Projectile projectileTemplate;

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

            StartCoroutine(FireProjectile());

            /// <summary>
            /// Deals damage at the appropriate interval.
            /// </summary>
            IEnumerator FireProjectile()
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

                    if (validTargets[0] == other)
                    {
                        onAttackStart?.Invoke();
                        Projectile projectile = Instantiate(projectileTemplate.gameObject).GetComponent<Projectile>();
                        projectile.transform.SetPositionAndRotation(projectileSource.position, projectileSource.rotation);
                        projectile.target = other.transform;
                        projectile.source = this;
                    }

                    yield return new WaitForSeconds(attackInterval - damageDelay);
                }
            }
        }
    }
}