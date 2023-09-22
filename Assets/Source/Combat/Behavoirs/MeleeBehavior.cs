using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ElementalEngagement.Combat
{
    /// <summary>
    /// Allows this to receive commands to melee attack enemies. 
    /// </summary>
    [RequireComponent(typeof(Selectable))]
    public class MeleeBehavior : CommandReceiver
    {
        [Tooltip("The area in which this will move towards unaligned health components.")]
        [SerializeField] private Collider visionRange;

        [Tooltip("The agent used to move this.")]
        [SerializeField] private NavMeshAgent agent;

        [Tooltip("The allegiance of this. Leave null for this to chase any object with a health component.")]
        [SerializeField] private Allegiance allegiance;


        /// <summary>
        /// Tests if an attack command is followable.
        /// </summary>
        /// <param name="hitUnderCursor"> The hit result from under the cursor. </param>
        /// <returns> True if the destination is reachable. </returns>
        public override bool CanExecuteCommand(RaycastHit hitUnderCursor)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// If the raycast hit a valid melee target, this will move towards that target until said target is killed 
        /// and then will move towards the closest valid target within its vision area until it the command is canceled.
        /// 
        /// If the raycast hit the ground and that location is naviable, this will move until it reaches the desired location or if it is unable to move certain period of time
        /// and then attack the closest unaligned health components within its attack area. 
        /// </summary>
        /// <param name="hitUnderCursor"> The hit result from under the cursor. </param>
        public override bool ExecuteCommand(RaycastHit hitUnderCursor)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Cancels any command this is currently performing.
        /// </summary>
        public override void CancelCommand()
        {
            throw new System.NotImplementedException();
        }
    }
}