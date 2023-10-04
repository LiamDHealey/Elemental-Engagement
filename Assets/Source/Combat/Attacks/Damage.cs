using ElementalEngagement.Favor;
using ElementalEngagement.Player;
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
        // The causer of the damage and the associated god.
        public Allegiance allegiance;

        /// <summary>
        /// Creates a damage with the given stats.
        /// </summary>
        /// <param name="amount"> The amount of damage being dealt in hp. </param>
        /// <param name="allegiance"> The causer of the damage and the associated god. </param>
        public Damage(float amount, Allegiance allegiance = null)
        {
            this.amount = amount;
            this.allegiance = allegiance;
        }
    }
}
