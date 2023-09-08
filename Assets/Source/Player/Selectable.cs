using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ElementalEngagement.Player
{
    /// <summary>
    /// Something that can be selected.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class Selectable : MonoBehaviour
    {
        [Tooltip("The name of the selection group this is in. Only selectable objects in the same group can be selected at the same time.")]
        [field: SerializeField] public string selectionGroup { get; private set; } = "Default";

        [Tooltip("Called when this has been selected. Can be invoked to select this object")]
        public UnityEvent<IEnumerable<Selectable>> onSelected;

        [Tooltip("Called when this has been deselected. Can be invoked to deselect this object.")]
        [SerializeField] public UnityEvent onDeselected;



        // Whether or not this is currently selected.
        public bool isSelected { get; private set; } = false;
    }
}
