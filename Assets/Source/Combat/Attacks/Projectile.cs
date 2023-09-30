using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ElementalEngagement.Combat
{
    public abstract class Projectile : MonoBehaviour
    {
        [Tooltip("The knockback source used by this")]
        [SerializeField] protected Transform knockbackSource;

        [Tooltip("Called when this projectile hits its target")]
        [SerializeField] protected UnityEvent onHit;

        // The target this projectile will hit.
        [System.NonSerialized] public Transform target;

        // The attack that acts as the source of this projectile
        public abstract ProjectileAttack source { get; set; }
    }
}