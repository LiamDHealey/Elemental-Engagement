using ElementalEngagement.Favor;
using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ElementalEngagement.Combat
{
    /// <summary>
    /// Stores data about a knockback event.
    /// </summary>
    [System.Serializable]
    public struct Knockback
    {
        [Tooltip("The distance to knock the hit thing back.")]
        public float amount;

        [Tooltip("The duration the hit thing will moved for.")] [Min(1/60)]
        public float duration;

        [Tooltip("The location the knockback is coming from.")]
        public Transform source;

        public Knockback(float amount, Transform source, float duration = 0.2f)
        {
            this.amount = amount;
            this.source = source;
            this.duration = duration;
        }
    }
}