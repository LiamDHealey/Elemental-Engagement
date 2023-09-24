using ElementalEngagement.Favor;
using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace ElementalEngagement.Combat
{
    /// <summary>
    /// Allows this to sacrifice itself at a sacrifice location.
    /// </summary>
    [RequireComponent(typeof(Selectable))]
    public class SacrificeCommand : CommandReceiver
    {
        [Tooltip("The agent used to move this.")]
        [SerializeField] private NavMeshAgent agent;

        [Tooltip("The allegiance of this. Can not be null.")]
        [SerializeField] private Allegiance allegiance;

        [Tooltip("The amount of favor gained by sacrificing this.")] [Range(-1, 1)]
        [SerializeField] private float sacrificeValue;


        /// <summary>
        /// Tests if an attack command is followable.
        /// </summary>
        /// <param name="hitUnderCursor"> The hit result from under the cursor. </param>
        /// <returns> True if the destination is a sacrifice location, and is aligned with this. </returns>
        public override bool CanExecuteCommand(RaycastHit hitUnderCursor)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Moves to the sacrifice location and then sacrifices itself to it by calling Sacrifice on the sacrifice location.
        /// </summary>
        /// <param name="hitUnderCursor"> The hit result from under the cursor. </param>
        public override void ExecuteCommand(RaycastHit hitUnderCursor)
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