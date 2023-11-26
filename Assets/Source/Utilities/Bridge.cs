using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[ExecuteAlways]
public class Bridge : MonoBehaviour
{
    public Vector2 segmentSize;

    public Vector2Int bridgeSize;

    public BoxCollider boxCollider;

    [Header("Segments")]
    public GameObject topLeftCorner;
    public GameObject topCenterEdge;
    public GameObject topRightCorner;
    public GameObject middleLeftEdge;
    public GameObject middleCenterFloor;
    public GameObject middleRightEdge;
    public GameObject bottomLeftCorner;
    public GameObject bottomCenterEdge;
    public GameObject bottomRightCorner;


    private void Start()
    {
        UpdateBridge();
    }
    public void UpdateBridge()
    {
        gameObject.layer = LayerMask.NameToLayer("Ground");


        while(transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        boxCollider.size = new Vector3(segmentSize.x * bridgeSize.x, 1, segmentSize.y * bridgeSize.y);
        boxCollider.center = new Vector3(0, -0.5f, 0);
        for (int x = 0; x < bridgeSize.x; x++)
        {
            for (int z = 0; z < bridgeSize.y; z++)
            {
                GameObject newSegment;
                if (x == 0)
                {
                    if (z == 0)
                    {
                        newSegment = Instantiate(topLeftCorner, transform);
                    }
                    else if (z == bridgeSize.y - 1)
                    {
                        newSegment = Instantiate(topRightCorner, transform);
                    }
                    else
                    {
                        newSegment = Instantiate(topCenterEdge, transform);
                    }
                }
                else if (x == bridgeSize.x - 1)
                {
                    if (z == 0)
                    {
                        newSegment = Instantiate(bottomLeftCorner, transform);
                    }
                    else if (z == bridgeSize.y - 1)
                    {
                        newSegment = Instantiate(bottomRightCorner, transform);
                    }
                    else
                    {
                        newSegment = Instantiate(bottomCenterEdge, transform);
                    }
                }
                else if (z == 0)
                {
                    newSegment = Instantiate(middleLeftEdge, transform);
                }
                else if (z == bridgeSize.y - 1)
                {
                    newSegment = Instantiate(middleRightEdge, transform);
                }
                else
                {
                    newSegment = Instantiate(middleCenterFloor, transform);
                }

                newSegment.transform.localPosition = new Vector3(segmentSize.x * x - boxCollider.size.x/2f + segmentSize.x/2, 0, segmentSize.y * z - boxCollider.size.z / 2f + segmentSize.y / 2);
            }
        }
    }
}
[CustomEditor(typeof(Bridge))]
class DecalMeshHelperEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Update"))
            ((Bridge)target).UpdateBridge();

        base.OnInspectorGUI();
    }
}
