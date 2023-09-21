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

        [Tooltip("Toggle to add the potential for chained knockbacks")]
        [SerializeField] private bool chainKnockback = true;

        //List of knockbacks, added together to create a final distance vector
        private List<CurrentKnockback> currentKnockbacks = new List<CurrentKnockback>();

        //Guarantees the KnockbackEnd unity event is only called once
        private bool endReported = true;


        /// <summary>
        /// Moves this away from the knockback source a given distance over a given amount of time.
        /// </summary>
        /// <param name="knockback"> The info of how this will be knocked back. </param>
        public void ReceiveKnockback(Knockback knockback)
        {
            if (knockback.amount == 0 || knockback.duration == 0)
            {
                return;
            }

            endReported = false;
            Vector3 direction = rigidbody.transform.position - knockback.source.position;
            direction.y = 0;
            direction.Normalize();

            if (chainKnockback == false)
            {
                if (currentKnockbacks.Count > 0) { return; }
                CurrentKnockback onlyKnockback = new CurrentKnockback(direction * knockbackMultiplier * knockback.amount / knockback.duration, knockback.duration);
                currentKnockbacks.Add(onlyKnockback);
                onKnockbackBegin?.Invoke();
                return;
            }

            CurrentKnockback newKnockback = new CurrentKnockback(direction * knockbackMultiplier * knockback.amount / knockback.duration, knockback.duration);
            currentKnockbacks.Add(newKnockback);
            onKnockbackBegin?.Invoke();
        }

        /// <summary>
        /// Update that adds all vectors together to create a total vector to move along
        /// </summary>
        private void FixedUpdate()
        {
            Vector3 totalKnockback = Vector3.zero;
            for(int i = 0; i < currentKnockbacks.Count; i++)
            {
                totalKnockback += currentKnockbacks[i].distanceOverTime;
                if ((currentKnockbacks[i].duration -= Time.fixedDeltaTime) > 0)
                {
                    continue;
                }
                currentKnockbacks.RemoveAt(i--);
            }

            if(currentKnockbacks.Count > 0)
            {
                Vector3 newPosition = transform.position + (totalKnockback * Time.fixedDeltaTime);

                transform.position = newPosition;
            }

            if (currentKnockbacks.Count == 0 && endReported == false)
            {
                onKnockbackEnd?.Invoke();
                endReported = true;
            }
        }

        /// <summary>
        /// Class to store all info for the currently calculated knockbacks
        /// </summary>
        private class CurrentKnockback
        {
            //
            public Vector3 distanceOverTime;

            public float duration;

            public CurrentKnockback(Vector3 distanceOverTime, float duration)
            {
                this.distanceOverTime = distanceOverTime;
                this.duration = duration;
            }
        }
    }
}