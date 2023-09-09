using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ElementalEngagement.Combat
{
    /// <summary>
    /// Receives MoveTo messages from the and will move until it reaches the desired location or if it is unable to move certain period of time
    /// and then attack the closest unaligned health components within its attack area. 
    /// </summary>
    [RequireComponent(typeof(Selectable))]
    public class MeleeControler : MonoBehaviour
    {
        [Tooltip("The area in which this will move towards unaligned health components.")]
        [SerializeField] private Collider visionRange;

        [Tooltip("The agent used to move this.")]
        [SerializeField] private NavMeshAgent agent;

        [Tooltip("The allegiance of this. Leave null for this to chase any object with a health component.")]
        [SerializeField] private Allegiance allegiance;

        [Tooltip("The time in seconds to cancel a move to command if considered stationary.")]
        [SerializeField] private float movementTimeout = 1f;

        [Tooltip("The maximum distance this is allowed to move for it to still be considered stationary.")]
        [SerializeField] private float movementTimeoutMaxDistance = 0.5f;

        /// <summary>
        /// Called when the selection manager broadcasts MoveTo.
        /// </summary>
        /// <param name="position"> The desired move location. </param>
        public void MoveTo(Vector3 position)
        {
            throw new System.NotImplementedException();
        }
    }
}