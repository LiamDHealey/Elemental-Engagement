using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ElementalEngagement.Combat
{
    /// <summary>
    /// Allows this to receive knockback.
    /// </summary>
    public class KnockbackReceiver : MonoBehaviour
    {
        [Tooltip("The amount the knockback distance will be multiplied by.")] [Min(0)]
        [SerializeField] private float knockbackMultiplier = 1;

        [Tooltip("The rigid body used for collisions.")]
        [SerializeField] private Rigidbody rigidbody;

        [Tooltip("Called when this starts being knocked back.")]
        [SerializeField] private UnityEvent onKnockbackBegin;

        [Tooltip("Called when this finishes being knocked back.")]
        [SerializeField] private UnityEvent onKnockbackEnd;

        /// <summary>
        /// Moves this away from the knockback source a given distance over a given amount of time.
        /// </summary>
        /// <param name="knockback"> The info of how this will be knocked back. </param>
        public void ReceiveKnockback(Knockback knockback)
        {
            onKnockbackBegin?.Invoke();
           
            Vector3 knockbackDirection = knockback.source.position - rigidbody.transform.position;
            knockbackDirection.Normalize();
            rigidbody.transform.eulerAngles = knockback.source.position;
            
            float timeElapsed = 0;
            
            while(timeElapsed < knockback.duration)
            {
                rigidbody.transform.position += (knockbackDirection * knockbackMultiplier) / knockback.duration;
                timeElapsed += Time.deltaTime;
            }

            onKnockbackEnd?.Invoke();
        }
    }
}