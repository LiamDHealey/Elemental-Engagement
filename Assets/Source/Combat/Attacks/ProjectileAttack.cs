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

        private float timeRemainingToAttack;

        //Private tracker for waitBeforeDamage that can be set to true to start the cycle after 
        //the initial check
        private bool needsToWait;

        /// <summary>
        /// Binds events
        /// </summary>
        private void Awake()
        {
            needsToWait = waitBeforeDamage;
            timeRemainingToAttack = attackInterval;
            attackRange.onTriggerEnter.AddListener(TriggerEntered);
            attackRange.onTriggerExit.AddListener( collider => validTargets.Remove(collider));
        }

        private void Update()
        {
            if (!enabled)
                return;
            if (validTargets.Count == 0)
            {
                if (timeRemainingToAttack > 0)
                    timeRemainingToAttack -= Time.deltaTime;
                else
                    timeRemainingToAttack = attackInterval;

                return;
            }

            if (needsToWait && timeRemainingToAttack > 0)
            {
                timeRemainingToAttack -= Time.deltaTime;
                return;
            }
            else if (timeRemainingToAttack <= 0 || !needsToWait)
            {
                while (validTargets.Count > 0 && validTargets[0] == null)
                {
                    validTargets.RemoveAt(0);
                }
                if(validTargets.Count == 0)
                    return;

                onAttackStart?.Invoke();
                Projectile projectile = Instantiate(projectileTemplate.gameObject).GetComponent<Projectile>();
                projectile.transform.SetPositionAndRotation(projectileSource.position, projectileSource.rotation);
                projectile.target = validTargets[0].transform;
                projectile.source = this;

                timeRemainingToAttack = attackInterval;
                needsToWait = true;
            }
            else
            {
                timeRemainingToAttack -= Time.deltaTime;
            }
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
        }
    }
}