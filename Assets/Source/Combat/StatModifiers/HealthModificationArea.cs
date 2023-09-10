using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElementalEngagement.Combat
{
    /// <summary>
    /// Used to buff/debuff health in an area.
    /// </summary>
    public class HealthModificationArea : StatModificationArea
    {
        [Tooltip("The amount to add to the max hp of any health in the area as a percent of its current value. When entering and leaving this area units stay at the same % of max health.")]
        [SerializeField] private float deltaMaxHp;
    }
}