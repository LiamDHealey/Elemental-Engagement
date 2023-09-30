using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ElementalEngagement.Combat
{
    public class GuaranteedHitProjectile : Projectile
    {
        [Tooltip("The speed of the projectile")]
        [SerializeField] private float speed = 20;

        // The attack that acts as the source of this projectile
        private ProjectileAttack _source = null;
        public override ProjectileAttack source 
        {
            get => _source;
            set
            {
                if (_source != null)
                {
                    Debug.LogWarning("Projectile source was set twice ");
                }

                _source = value;

                knockback = _source.knockback;
                if (knockback.source == null)
                    knockback.source = knockbackSource;

                damage = _source.damage;
            }
        }

        // The knockback this will deal.
        private Knockback knockback;

        // The damage this will deal.
        private Damage damage;

        private bool hit = false;

        /// <summary>
        /// Moves the projectile and hits the target when necessaries.
        /// </summary>
        private void FixedUpdate()
        {
            if (hit)
                return;

            if (target == null)
            {
                Destroy(gameObject);
                return;
            }

            Vector3 distanceToTarget = (target.position - transform.position);

            if (distanceToTarget.sqrMagnitude < speed * speed * Time.fixedDeltaTime * Time.fixedDeltaTime)
            {
                target.GetComponent<Health>()?.TakeDamage(damage);
                target.GetComponent<KnockbackReceiver>()?.ReceiveKnockback(knockback);

                hit = true;
                onHit?.Invoke();
            }
            else
            {
                transform.forward = distanceToTarget.normalized;
                transform.position += distanceToTarget.normalized * Time.fixedDeltaTime * speed;
            }
        }
    }
}