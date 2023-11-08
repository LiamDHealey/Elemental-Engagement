using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using static UnityEngine.RuleTile.TilingRuleOutput;
using Object = UnityEngine.Object;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{


    [Tooltip("The nav mesh this will use.")]
    public int agentType = 0;
    [Tooltip("The mask used to snap to the ground.")]
    public LayerMask walkableMask;
    [Tooltip("The speed this will move at")]
    [SerializeField] private float _speed = 10;
    public float speed 
    {
        get => _speed; 
        set
        {
            _speed = value;
            agent.speed = value;
        }
    }

    // Whether or not this can curranty move
    public bool canMove => movementPreventers.Count == 0;
    // Called when something prevents this from moving.
    public UnityEvent onMovementPrevented;
    // Called when this is no longer being prevented from moving.
    public UnityEvent onMovementResumed;

    // Whether or not this has reached its destination.
    public bool destinationReached { get; private set; } = true;
    // Called when this reaches its destination. Provided that this was not already at the destination.
    public UnityEvent onDestinationReached;


    private NavMeshAgent agent;
    private new Rigidbody rigidbody;
    private HashSet<Object> movementPreventers = new HashSet<Object>();
    private Object navigator = null;

    /// <summary>
    /// Prevents this movement from moving until allow movement is called with the same preventer.
    /// </summary>
    /// <param name="preventer"> The thing that is preventing this from moving.</param>
    public void PreventMovement(Object preventer)
    {
        if (preventer == null)
            throw new Exception("Preventer must be valid.");
        
        if (canMove)
        {
            onMovementPrevented?.Invoke();
        }

        movementPreventers.Add(preventer);
    }


    /// <summary>
    /// Removes the movement lock caused by the preventer. This may still be unable to move due to other movement preventers.
    /// </summary>
    /// <param name="preventer"> The thing that was preventing this from moving.</param>
    public void AllowMovement(Object preventer)
    {
        movementPreventers.Remove(preventer);

        if (canMove)
        {
            onMovementResumed?.Invoke();
        }
    }

    /// <summary>
    /// Sets the destination of this movement component
    /// </summary>
    /// <param name="navigator"> The object responsible for setting this destination. </param>
    /// <param name="destination"> The location this will move to. </param>
    /// <param name="stoppingDistance"> The distance within the destination which this will stop at. </param>
    /// <exception cref="Exception"></exception>
    public void SetDestination(Object navigator, Vector3 destination, float stoppingDistance = 1)
    {
        if (navigator == null)
            throw new Exception("Navigator must be valid.");

        if (agent == null)
        {
            StartCoroutine(CallAgain());
            IEnumerator CallAgain()
            {
                while (agent == null)
                {
                    yield return null;
                }
                SetDestination(navigator, destination, stoppingDistance);
            }
            return;
        }

        agent.stoppingDistance = stoppingDistance;
        agent.SetDestination(destination);
        this.navigator = navigator;

        destinationReached = (destination - transform.position).magnitude <= stoppingDistance;
    }

    /// <summary>
    /// Cancels the last destination that was set.
    /// </summary>
    /// <param name="navigator"> The navigator that set the destination. </param>
    /// <exception cref="Exception"></exception>
    public void RemoveDestination(Object navigator)
    {
        if (navigator == null)
            throw new Exception("Navigator must be valid.");

        if (this.navigator == navigator)
        {
            this.navigator = null;
        }
    }

    /// <summary>
    /// Gets if this can move to a location.
    /// </summary>
    /// <param name="destination"> The location to test. </param>
    /// <returns> True is this can reach destination. </returns>
    public bool CanMoveTo(Vector3 destination)
    {
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(destination, path);
        return path.status == NavMeshPathStatus.PathComplete;
    }

    
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        if (!NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 100f, NavMesh.AllAreas))
        {
            Debug.LogError("Movement component too far from mesh");
            Invoke("Start", 1);
            return;
        }
        transform.position = hit.position;
        agent = gameObject.AddComponent<NavMeshAgent>();
        agent.updatePosition = false;
        agent.updateRotation = false;

        agent.agentTypeID = NavMesh.GetSettingsByIndex(agentType).agentTypeID;
        agent.acceleration = 99999f;

    }

    private void Update()
    {
        if (!destinationReached && canMove && navigator != null)
        {
            if (Physics.Raycast(transform.position + Vector3.up * 50, Vector3.down, out RaycastHit hit, 500f, walkableMask))
            {
                transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
            }
            transform.position = (transform.position + (Vector3.Scale(agent.desiredVelocity, new Vector3(1, 0, 1)).normalized) * Mathf.Min(agent.remainingDistance, speed * Time.deltaTime));
            agent.nextPosition = transform.position;
        }

        if (!destinationReached && navigator != null && agent.stoppingDistance >= (agent.destination - transform.position).magnitude)
        {
            destinationReached = true;
            onDestinationReached?.Invoke();
            RemoveDestination(navigator);
        }
    }
}
