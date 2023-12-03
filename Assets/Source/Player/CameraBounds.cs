using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CameraBounds : MonoBehaviour
{
    public static BoxCollider boundsBox { get; private set; }

    private void Start()
    {
        boundsBox = GetComponent<BoxCollider>();
    }
}
