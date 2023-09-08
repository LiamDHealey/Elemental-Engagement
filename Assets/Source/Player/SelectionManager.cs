using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace ElementalEngagement.Player
{
    /// <summary>
    /// Handles the selection of selectable objects. Can select either by clicking on an individual selectable 
    /// or by clicking and dragging to create a selection box which will select all members of the selection group with the most member within the box.
    /// </summary>
    [RequireComponent(typeof(Allegiance))]
    public class SelectionManager : MonoBehaviour
    {
        [Tooltip("The input component that this will get mouse data from.")]
        [SerializeField] private PlayerInput input;

        [Tooltip("The camera from which to make the selection.")]
        [SerializeField] private new Camera camera;

        [Tooltip("The camera from which to make the selection.")]
        [SerializeField] private Canvas hudCanvas;

        [Tooltip("The camera from which to make the selection.")]
        [SerializeField] private RectTransform selectionBox;

        [Tooltip("The minimum distance from initial click location to create a selection box for.")]
        [SerializeField] private float minBoxSize = 10;
    }
}