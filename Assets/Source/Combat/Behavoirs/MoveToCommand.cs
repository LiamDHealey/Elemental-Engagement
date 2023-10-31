using ElementalEngagement.Favor;
using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
        [Tooltip("The allegiance of this. Leave null for this to chase any object with a health component.")]
        [SerializeField] private Allegiance allegiance;

        [Tooltip("The agent used to move this.")]
        [SerializeField] private NavMeshAgent agent;

        [Tooltip("The area in which this will move towards unaligned health components when attack moving.")]
        [SerializeField] private BindableCollider visionRange;

        [Tooltip("If the unit moves slower than this, it will consider itself stationary.")]
        [SerializeField] private float minMovementSpeed = 0.5f;

        [Tooltip("If the unit is stationary for this long the command will be canceled.")]
        [SerializeField] private float minMovementSpeedTimeout = 1f;

        [Tooltip("The distance away from the closet attack target to stop at when attack moving.")] [Min(0)]
        [SerializeField] private float stoppingRange = 0f;


        // The targets currently in range.
        HashSet<Transform> validTargets = new HashSet<Transform>();

        private void Start()
        {
            visionRange.onTriggerEnter.AddListener(
                (collider) =>
                {
                    if (collider.GetComponent<Health>() != null 
                        && !allegiance.CheckFactionAllegiance(collider.GetComponent<Allegiance>()))
                    {
                        validTargets.Add(collider.transform);
                    }
                });
            visionRange.onTriggerExit.AddListener(
                (collider) => validTargets.Remove(collider.transform));
        }

        /// <summary>
        /// Tests if an move to command is followable.
        /// </summary>
        /// <param name="hitUnderCursor"> The hit result from under the cursor. </param>
        /// <returns> True if the destination is a sacrifice location, and is aligned with this. </returns>
        public override bool CanExecuteCommand(RaycastHit hitUnderCursor)
        {
            if (hitUnderCursor.collider == null)
                return false;

            if (hitUnderCursor.collider.gameObject.layer != LayerMask.NameToLayer("Ground"))
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


            StartCoroutine(DestinationReached());
            IEnumerator DestinationReached()
            {
                // The time at which this started being stationary.
                float? startStationaryTime = null;
                do
                {
                    // Exclude targets outside the starting vision area.
                    IEnumerable<Transform> attackableTargets = validTargets.Where(t => t != null);

                    // Move to target location
                    if (!isAltCommand || attackableTargets.Count() == 0)
                    {
                        agent.isStopped = false;
                        agent.MoveTo(hitUnderCursor.point);
                        Debug.DrawRay(hitUnderCursor.point, Vector3.up, Color.red, 1);

                        // Min Movement Speed Timeout
                        Vector3 lastPosition = agent.transform.position;
                        yield return null;
                        if ((lastPosition -  agent.transform.position).magnitude/Time.deltaTime < minMovementSpeed)
                        {
                            if (startStationaryTime == null)
                            {
                                startStationaryTime = Time.time;
                            }
                            else if (Time.time - startStationaryTime > minMovementSpeedTimeout)
                            {
                                break;
                            }
                        }
                        else
                        {
                            startStationaryTime = null;
                        }
                    }
                    // Chase targets
                    else
                    {
                        // Get the closest target
                        float SqrDistance(Transform target) => (target.position - transform.position).sqrMagnitude;
                        Transform closetsTarget = attackableTargets
                            .Aggregate((closest, next) => 
                                SqrDistance(closest) < SqrDistance(next) ? next : closest);


                        agent.isStopped = SqrDistance(closetsTarget) < stoppingRange * stoppingRange;
                        agent.MoveTo(closetsTarget.position);

                        yield return null;
                    }
                }
                while (commandInProgress);

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