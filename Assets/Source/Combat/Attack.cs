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
        public ElementalEngagement.Player.Allegiance allegiance;

        [Tooltip("The damage dealt by this.")]
        [SerializeField] Damage damage;

        [Tooltip("A set of events that will be at called at different time offsets from when this attacks. Useful for animations and sound effects")]
        [SerializeField] private List<Event> onAttack;

        [System.Serializable]
        private class Event
        {
            [Tooltip("The time in seconds before (-) or after (+) onAttack when this will be called.")]
            public float timeOffset = 0;

            [Tooltip("Called a certain amount of time before or after on attack.")]
            public UnityEvent onTriggered;
        }
    }
}