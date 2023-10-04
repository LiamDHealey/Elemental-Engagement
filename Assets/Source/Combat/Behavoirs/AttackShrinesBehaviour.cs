using ElementalEngagement;
using ElementalEngagement.Combat;
using ElementalEngagement.Favor;
using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// Cause this to move to shrines when not being commanded
/// </summary>
public class AttackShrinesBehaviour : MonoBehaviour
{
    [Tooltip("The agent used to move this.")]
    [SerializeField] private NavMeshAgent agent;

    [Tooltip("The allegiance of this. Leave null for this to chase any object with a health component.")]
    [SerializeField] private Allegiance allegiance;

    [Tooltip("The distance away from the closet target to stop at.")][Min(0)]
    [SerializeField] private float stoppingRange = 0f;

    // Start is called before the first frame update
    void Start()
    {
        List<Transform> shrines = FindObjectsOfType<Spawner>()
            .Where(spawner => spawner.GetComponent<Health>()    != null)
            .Where(spawner => !spawner.GetComponent<Allegiance>().CheckFactionAllegiance(allegiance))
            .Select(spawner => spawner.transform)
            .ToList();

        foreach (Transform shrine in shrines)
        {
            shrine.GetComponent<Health>().onDamaged.AddListener(delegate { shrines.Remove(shrine); });
        }

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

                // Attacks any target in its original vision range until commanded.
                // Will also move back to its initial position when no targets are visible.
                yield return StartCoroutine(AttackTarget());
                IEnumerator AttackTarget()
                {
                    while (!receivers.Any(r => r.commandInProgress) && shrines.Count > 0)
                    {
                        float SqrDistance(Transform target) => (target.position - transform.position).sqrMagnitude;

                        // Get the closest target
                        Transform closetsTarget = shrines
                            .Aggregate((closest, next) =>
                            {
                                if (SqrDistance(closest) > SqrDistance(next))
                                    return next;
                                else
                                    return closest;
                            });

                        agent.isStopped = SqrDistance(closetsTarget) < stoppingRange * stoppingRange;
                        agent.MoveTo(closetsTarget.GetComponent<Spawner>().spawnLocation.position);
                        Debug.Log(agent.pathStatus);    
                        yield return null;
                    }
                }
            }
        }
    }
}
