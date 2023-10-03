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
    /// Allows this to move to the commanded location.
    /// </summary>
    [RequireComponent(typeof(Selectable))]
    public class MoveToCommand : CommandReceiver
    {
        [Tooltip("The agent used to move this.")]
        [SerializeField] private NavMeshAgent agent;

        [Tooltip("The time in seconds to cancel a move to command if considered stationary.")]
        [SerializeField] private float movementTimeout = 1f;

        [Tooltip("The maximum distance this is allowed to move for it to still be considered stationary.")]
        [SerializeField] private float movementTimeoutMaxDistance = 0.5f;

        /// <summary>
        /// Tests if an move to command is followable.
        /// </summary>
        /// <param name="hitUnderCursor"> The hit result from under the cursor. </param>
        /// <returns> True if the destination is a sacrifice location, and is aligned with this. </returns>
        public override bool CanExecuteCommand(RaycastHit hitUnderCursor)
        {
            if (hitUnderCursor.collider == null)
                return false;

            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(hitUnderCursor.point, path);
            return path.status == NavMeshPathStatus.PathComplete;
        }

        /// <summary>
        /// Moves to the hit location.
        /// </summary>
        /// <param name="hitUnderCursor"> The hit result from under the cursor. </param>
        public override void ExecuteCommand(RaycastHit hitUnderCursor)
        {
            agent.isStopped = false;
            commandInProgress = true;
            agent.MoveTo(hitUnderCursor.point);


            StartCoroutine(DestinationReached());
            IEnumerator DestinationReached()
            {
                Vector3 lastPosition = agent.transform.position;

                do
                {
                    bool PassedMovementThreshold()
                    {
                        bool result = (lastPosition - agent.transform.position).sqrMagnitude > movementTimeoutMaxDistance * movementTimeoutMaxDistance;
                        lastPosition = agent.transform.position;
                        return result;
                    }


                    if (!PassedMovementThreshold())
                    {
                        yield return new WaitForSeconds(movementTimeout);
                        if (!PassedMovementThreshold())
                            break;
                    }
                    yield return null;
                }
                while (commandInProgress && agent.remainingDistance > movementTimeoutMaxDistance);

                CancelCommand();
            }
        }

        /// <summary>
        /// Cancels any command this is currently performing.
        /// </summary>
        public override void CancelCommand()
        {
            StopAllCoroutines();
            agent.isStopped = true;
            commandInProgress = false;
        }
    }
}