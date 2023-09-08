using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElementalEngagement.Combat
{
    /// <summary>
    /// Deals damage to things in an area.
    /// </summary>
    public class AOEAttack : Attack
    {
        [Tooltip("The collider in which the target must be within to take damage.")]
        [SerializeField] private Collider attackRanage;

        [Tooltip("The time between dealing damages in seconds.")] [Min(1/60f)]
        [SerializeField] private float damageRate = 0.5f;

        [Tooltip("Whether or not to wait a single iteration when a new target enters the attack range..")]
        [SerializeField] private bool waitBeforeDamage = false;

        [Tooltip("The maximum number of things this can hit at once.")] [Min(1)]
        [SerializeField] private int maxTarget = 1;
    }
}