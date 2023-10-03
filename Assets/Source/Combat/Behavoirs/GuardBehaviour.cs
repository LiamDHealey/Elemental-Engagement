using ElementalEngagement.Player;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace ElementalEngagement.Combat
{
    /// <summary>
    /// Allows makes this move to enemies within vision range 
    /// </summary>
    public class GuardBehaviour : MonoBehaviour
    {
        [Tooltip("The area in which this will move towards unaligned health components.")]
        [SerializeField] private BindableCollider visionRange;

        [Tooltip("The agent used to move this.")]
        [SerializeField] private NavMeshAgent agent;

        [Tooltip("The allegiance of this. Leave null for this to chase any object with a health component.")]
        [SerializeField] private Allegiance allegiance;

        [Tooltip("The distance away from the closet target to stop at.")] [Min(0)]
        [SerializeField] private float stoppingRange = 0f;

        /// <summary>
        /// Moves this to the closes unaligned game object with a health component when not being commanded.
        /// Will stay within its the area it could initially see once all commands were completed.
        /// </summary>
        private void Start()
        {
            // Track valid targets.
            List<Transform> validTargets = new List<Transform>();
            visionRange.onTriggerEnter.AddListener(
                collider =>
                {
                    if (!allegiance.CheckFactionAllegiance(collider.GetComponent<Allegiance>()) // Is not aligned
                        && collider.GetComponent<Health>() != null) // Is attackable)
                    {
                        validTargets.Add(collider.transform);
                    }
                });
            visionRange.onTriggerExit.AddListener(
                collider => { validTargets.Remove(collider.transform); } );

            // Waits until this is idle then moves to visible targets.
            StartCoroutine(AttackWhenIdle());
            IEnumerator AttackWhenIdle()
            {
                IEnumerable<CommandReceiver> receivers = GetComponents<CommandReceiver>();

                while (true)
                {
                    yield return null;

                    if (receivers.Any(r => r.commandInProgress))
                        continue;

                    Bounds anchorBounds = visionRange.GetComponent<Collider>().bounds;

                    // Attacks any target in its original vision range until commanded.
                    // Will also move back to its initial position when no targets are visible.
                    yield return StartCoroutine(AttackTarget());
                    IEnumerator AttackTarget()
                    {
                        while (!receivers.Any(r => r.commandInProgress))
                        {
                            // Exclude targets outside the starting vision area.
                            IEnumerable<Transform> attackableTargets = validTargets
                                    .Where(target => target != null && anchorBounds.Contains(target.transform.position));

                            // Return to starting area
                            if (attackableTargets.Count() == 0)
                            {
                                agent.isStopped = false;
                                agent.MoveTo(anchorBounds.center);
                            }
                            // Chase targets
                            else
                            {
                                float SqrDistance(Transform target) => (target.position - transform.position).sqrMagnitude;

                                // Get the closest target
                                Transform closetsTarget = attackableTargets
                                    .Aggregate((closest, next) =>
                                    {
                                        if (SqrDistance(closest) < SqrDistance(next))
                                            return next;
                                        else
                                            return closest;
                                    });

                                agent.isStopped = SqrDistance(closetsTarget) < stoppingRange * stoppingRange;
                                agent.MoveTo(closetsTarget.position);
                            }

                            yield return null;
                        }
                    }
                }
            }
        }
    }
}