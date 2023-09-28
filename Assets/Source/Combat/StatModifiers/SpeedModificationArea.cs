using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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


        void Start()
        {

            area.onTriggerEnter.AddListener(OnTriggerEnter);
            area.onTriggerExit.AddListener(OnTriggerExit);

            /// <summary>
            /// Activates when an object enters the object's collider area.
            /// If the object is a unit, then apply the speed upgrade to that unit.
            /// </summary>
            /// <param name="other"></param>
            void OnTriggerEnter(Collider other)
            {

                Allegiance all = other.GetComponent<Allegiance>();

                NavMeshAgent speed = other.GetComponent<NavMeshAgent>();

                if (speed == null || all == null)
                    return;

                // If collider allegiance does not equal area of effect's allegiance, or the area of effect's allegiance is unaligned.
                if (all.faction != allegiance.faction || allegiance.faction == Faction.Unaligned)
                    return;

                // If collider god is not equal to the area of effect's god, or the area of effect's god is unaligned.
                if (all.god != allegiance.god || allegiance.god == Favor.MinorGod.Unaligned)
                    return;

                speed.speed *= deltaSpeed;
                speed.angularSpeed *= deltaAngularSpeed;
                speed.acceleration *= deltaAcceleration;
            }

            /// <summary>
            /// Activates when an object exits the object's collider area.
            /// If the object is a unit, then apply the speed downgrade to that unit.
            /// </summary>
            /// <param name="other"></param>
            void OnTriggerExit(Collider other)
            {

                Allegiance all = other.GetComponent<Allegiance>();

                NavMeshAgent speed = other.GetComponent<NavMeshAgent>();

                if (speed == null || all == null)
                    return;

                // If collider allegiance does not equal area of effect's allegiance, or the area of effect's allegiance is unaligned.
                if (all.faction != allegiance.faction || allegiance.faction == Faction.Unaligned)
                    return;

                // If collider god is not equal to the area of effect's god, or the area of effect's god is unaligned.
                if (all.god != allegiance.god || allegiance.god == Favor.MinorGod.Unaligned)
                    return;

                speed.speed /= deltaSpeed;
                speed.angularSpeed /= deltaAngularSpeed;
                speed.acceleration /= deltaAcceleration;
            }
        }
    }
}
