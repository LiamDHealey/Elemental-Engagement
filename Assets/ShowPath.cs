using ElementalEngagement.Combat;
using ElementalEngagement.Favor;
using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class ShowPath : MonoBehaviour
{
    private Transform target;
    private NavMeshPath path;
    private Allegiance allegiance;
    private LineRenderer lineRenderer;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        path = new NavMeshPath();
        allegiance = GetComponent<Allegiance>();

        List<Transform> shrines = FindObjectsOfType<Spawner>()
            .Where(spawner => spawner.GetComponent<Health>() != null)
            .Where(spawner => spawner.GetComponent<Allegiance>().CheckFactionAllegiance(allegiance))
            .Select(spawner => spawner.transform)
            .ToList();

        foreach(Transform shrine in shrines)
        {
            if(shrine.GetComponent<Allegiance>().god == allegiance.god)
            {
                target = shrine;
            }
        }
    }

    void Update()
    {

        // Update the way to the goal every second.
        NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);

        Vector3[] newPositions = path.corners;
        for(int i = 0; i < newPositions.Length; i++)
        {
            newPositions[i].y += 10;
        }

        lineRenderer.positionCount = newPositions.Count();
        lineRenderer.SetPositions(newPositions);

        for (int i = 0; i < path.corners.Length - 1; i++)
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red, 1000f);
    }
}
