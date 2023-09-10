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
        [SerializeField] private ElementalEngagement.Player.Allegiance allegiance;

        [Tooltip("The damage dealt by this.")]
        public Damage damage;

        [Tooltip("The knockback dealt by this.")]
        public Knockback knockback;
        
        [Tooltip("The time between dealing damages in seconds.")] [Min(1/60f)]
        public float attackInterval = 0.5f;

        [Tooltip("A set of events that will be at called at different time offsets from when this attacks. Useful for animations and sound effects")]
        public List<Event> onAttack;

        /// <summary>
        /// Represents a thing that can be triggered before or after an attack.
        /// </summary>
        [System.Serializable]
        public class Event
        {
            [Tooltip("The time in seconds before (-) or after (+) onAttack when this will be called.")]
            public float timeOffset = 0;

            [Tooltip("Called a certain amount of time before or after on attack.")]
            public UnityEvent onTriggered;
        }
    }
}