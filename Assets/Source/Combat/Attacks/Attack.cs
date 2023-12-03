using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ElementalEngagement.Combat
{
    /// <summary>
    /// Something that can deal damage.
    /// </summary>
    public abstract class Attack : MonoBehaviour
    {
        [Tooltip("The component determining the allegiance of this. Will not damage objects that are aligned. Will damage any object if null.")]
        public Player.Allegiance allegiance;

        [Tooltip("The damage dealt by this.")]
        public Damage damage;

        [Tooltip("The knockback dealt by this.")]
        public Knockback knockback;
        
        [Tooltip("The time between starting attacks in seconds.")] [Min(1/60f)]
        public float attackInterval = 0.5f;
        
        [Tooltip("The time between stating an attack and dealing damage. Must be less than attackInterval")] [Min(0f)]
        public float damageDelay = 0f;

        [Tooltip("Whether or not to wait a for attack rate to elapse once when a new target enters the attack range.")]
        public bool waitBeforeDamage = false;

        [Tooltip("Called when this starts its attack")]
        public UnityEvent onAttackStart;

        [Tooltip("Called when this attack actually deals damage")]
        public UnityEvent onAttackDamage;

        public abstract void SetAttackInterval(float newAttackInterval, bool waitAfterChanging);
    }
}