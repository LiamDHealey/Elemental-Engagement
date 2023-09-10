using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElementalEngagement.Combat
{
    /// <summary>
    /// Used to buff/debuff navmesh agents in an area.
    /// </summary>
    public class SpeedModificationArea : StatModificationArea
    {
        [Tooltip("The amount to add to the speed of any navmesh agents in the area as a percent of its current value.")]
        [SerializeField] private float deltaSpeed;

        [Tooltip("The amount to add to the angular speed of any navmesh agents in the area as a percent of its current value.")]
        [SerializeField] private float deltaAngularSpeed;

        [Tooltip("The amount to add to the acceleration of any navmesh agents in the area as a percent of its current value.")]
        [SerializeField] private float deltaAcceleration;
    }
}