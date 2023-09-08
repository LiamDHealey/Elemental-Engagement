using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ElementalEngagement.Combat
{
    /// <summary>
    /// Stores data about a damage event.
    /// </summary>
    [System.Serializable]
    public struct Damage
    {
        // The amount of damage being dealt in hp.
        public float amount;
        // The type of the damage being dealt.
        public Type type;
        // The entity that causing the damage.
        public GameObject causer;

        /// <summary>
        /// Creates a damage with the given stats.
        /// </summary>
        /// <param name="amount"> The amount of damage being dealt in hp. </param>
        /// <param name="type"> The type of the damage being dealt. </param>
        /// <param name="causer"> The entity that causing the damage. </param>
        public Damage(float amount, Type type, GameObject causer)
        {
            this.amount = amount;
            this.type = type;
            this.causer = causer;
        }

        /// <summary>
        /// The elemental type of a damage event.
        /// </summary>
        public enum Type
        {
            Untyped,
            Fire,
            Water,
            Earth,
        }
    }
}