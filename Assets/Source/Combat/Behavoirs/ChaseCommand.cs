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
    /// Allows this to chase the selected object.
    /// </summary>
    [RequireComponent(typeof(Selectable))]
    public class ChaseCommand : CommandReceiver
    {
        [Tooltip("The agent used to move this.")]
        [SerializeField] private NavMeshAgent agent;

        [Tooltip("The distance away from the chase target to stop.")]
        [SerializeField] private float stoppingDistance = 1f;

        [Tooltip("The interval on which the path is refreshed.")]
        [SerializeField] private float pathRefreshRate = 0.25f;

        /// <summary>
        /// Tests if an move to command is followable.
        /// </summary>
        /// <param name="hitUnderCursor"> The hit result from under the cursor. </param>
        /// <returns> True if the destination is a sacrifice location, and is aligned with this. </returns>
        public override bool CanExecuteCommand(RaycastHit hitUnderCursor)
        {
            if (hitUnderCursor.collider == null)
                return false;
            if (hitUnderCursor.collider.GetComponent<Health>() == null)
                return false;
            if (hitUnderCursor.collider.GetComponent<Allegiance>().CheckFactionAllegiance(GetComponent<Allegiance>()))
                return false;

            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(hitUnderCursor.point, path);
            return path.status == NavMeshPathStatus.PathComplete;
        }

        /// <summary>
        /// Moves to the hit location.
        /// </summary>
        /// <param name="hitUnderCursor"> The hit result from under the cursor. </param>
        /// <param name="isAltCommand"> Whether or not this should execute the alternate version of this command (if it exists). </param>
        public override void ExecuteCommand(RaycastHit hitUnderCursor, bool isAltCommand)
        {
            agent.isStopped = false;
            commandInProgress = true;

            StartCoroutine(ChaseTarget());
            IEnumerator ChaseTarget()
            {
                while (commandInProgress && hitUnderCursor.collider != null)
                {
                    Vector3 targetPosition = hitUnderCursor.collider.transform.position;

                    if ((targetPosition - transform.position).sqrMagnitude > stoppingDistance * stoppingDistance)
                    {
                        agent.isStopped = false;
                        agent.MoveTo(targetPosition);
                    } 
                    else
                    {
                        agent.isStopped = true;
                    }

                    yield return new WaitForSeconds(pathRefreshRate);
                }

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