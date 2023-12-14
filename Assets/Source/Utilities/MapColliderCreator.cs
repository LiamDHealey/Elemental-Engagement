using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class MapColliderCreator : MonoBehaviour
{
    public void Create()
    {
        NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();
        List<Vector3> vertices = triangulation.indices
            .Select(i => triangulation.vertices[i])
            .ToList();

        HashSet<(Vector3, Vector3)> edges = new HashSet<(Vector3, Vector3)>();
        for (int i = 0; i < vertices.Count - 1; i+=3)
        {
            edges.Add((vertices[i], vertices[i + 1]));
            edges.Add((vertices[i + 1], vertices[i + 2]));
            edges.Add((vertices[i + 2], vertices[i]));
        }

        
        foreach ((Vector3, Vector3) edge in edges.Where(e1 => !edges.Any(e2 => NearlyEqual(e2.Item1, e1.Item2) && NearlyEqual(e2.Item2, e1.Item1))))
        {
            Vector3 direction = (edge.Item1 - edge.Item2).normalized;
            Vector3 perpendicularDirection = Quaternion.AngleAxis(90, Vector3.up) * direction;
            Vector3 v1 = edge.Item1 + perpendicularDirection;
            Vector3 v2 = edge.Item2 + perpendicularDirection;

            BoxCollider box = new GameObject("MapCollider", typeof(BoxCollider)).GetComponent<BoxCollider>();
            box.transform.SetParent(transform, false);
            box.transform.position = (v1 + v2) / 2f + Vector3.up * 2.5f;
            box.transform.rotation = Quaternion.LookRotation(direction);
            box.size = new Vector3(1, 5, (v1 - v2).magnitude);
        }

        bool NearlyEqual(Vector3 v1, Vector3 v2) => (v1 - v2).sqrMagnitude < 0.1;
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(MapColliderCreator))]
class MapColliderCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Create"))
            ((MapColliderCreator)target).Create();

        base.OnInspectorGUI();
    }
}
#endif