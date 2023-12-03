using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[ExecuteAlways]
public class Bridge : MonoBehaviour
{
    public Vector2 segmentSize;

    public Vector2Int bridgeSize;

    public float railingWidth;

    public Transform segmentContainer;

    public BoxCollider boxCollider;

    public BoxCollider leftRailingCollider;

    public BoxCollider rightRailingCollider;

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

        int i = 0;
        while(segmentContainer.childCount > 0)
        {
            DestroyImmediate(segmentContainer.GetChild(i).gameObject);
        }

        boxCollider.size = new Vector3(segmentSize.x * bridgeSize.x, 1, segmentSize.y * bridgeSize.y);
        boxCollider.center = new Vector3(0, -0.5f, 0);
        leftRailingCollider.size = new Vector3(segmentSize.x * bridgeSize.x, 2, railingWidth);
        leftRailingCollider.center = new Vector3(0, 1, (segmentSize.y * bridgeSize.y + railingWidth) / 2f);
        rightRailingCollider.size = new Vector3(segmentSize.x * bridgeSize.x, 2, railingWidth);
        rightRailingCollider.center = new Vector3(0, 1, (segmentSize.y * bridgeSize.y + railingWidth) / -2f);

        for (int x = 0; x < bridgeSize.x; x++)
        {
            for (int z = 0; z < bridgeSize.y; z++)
            {
                GameObject newSegment;
                if (x == 0)
                {
                    if (z == 0)
                    {
                        newSegment = Instantiate(topLeftCorner, segmentContainer);
                    }
                    else if (z == bridgeSize.y - 1)
                    {
                        newSegment = Instantiate(topRightCorner, segmentContainer);
                    }
                    else
                    {
                        newSegment = Instantiate(topCenterEdge, segmentContainer);
                    }
                }
                else if (x == bridgeSize.x - 1)
                {
                    if (z == 0)
                    {
                        newSegment = Instantiate(bottomLeftCorner, segmentContainer);
                    }
                    else if (z == bridgeSize.y - 1)
                    {
                        newSegment = Instantiate(bottomRightCorner, segmentContainer);
                    }
                    else
                    {
                        newSegment = Instantiate(bottomCenterEdge, segmentContainer);
                    }
                }
                else if (z == 0)
                {
                    newSegment = Instantiate(middleLeftEdge, segmentContainer);
                }
                else if (z == bridgeSize.y - 1)
                {
                    newSegment = Instantiate(middleRightEdge, segmentContainer);
                }
                else
                {
                    newSegment = Instantiate(middleCenterFloor, segmentContainer);
                }

                newSegment.transform.localPosition = new Vector3(segmentSize.x * x - boxCollider.size.x/2f + segmentSize.x/2, 0, segmentSize.y * z - boxCollider.size.z / 2f + segmentSize.y / 2);
            }
        }
    }
}

#if UNITY_EDITOR
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
#endif