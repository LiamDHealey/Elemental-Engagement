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
        [Tooltip("The amount to multiply the speed of any units in this area by.")]
        [Min(0.00001f)]
        [SerializeField] private float speedMultiplier = 1;


        void Start()
        {
            area.onTriggerEnter.AddListener(OnTriggerEntered);
            area.onTriggerExit.AddListener(OnTriggerExited);
        }

        public void OnDestroy()
        {
            foreach (Collider collider in area.overlappingColliders)
            {
                OnTriggerExited(collider);
            }
        }

        /// <summary>
        /// Activates when an object enters the object's collider area.
        /// If the object is a unit, then apply the speed upgrade to that unit.
        /// </summary>
        /// <param name="collider"></param>
        void OnTriggerEntered(Collider collider)
        {
            Movement speed = collider.GetComponent<Movement>();

            if (speed == null)
                return;
            if (!CanModify(collider))
                return;

            speed.speed *= speedMultiplier;
        }

        /// <summary>
        /// Activates when an object exits the object's collider area.
        /// If the object is a unit, then apply the speed downgrade to that unit.
        /// </summary>
        /// <param name="collider"></param>
        void OnTriggerExited(Collider collider)
        {
            Movement speed = collider.GetComponent<Movement>();

            if (speed == null)
                return;
            if (!CanModify(collider))
                return;

            speed.speed /= speedMultiplier;
        }
    }
}
