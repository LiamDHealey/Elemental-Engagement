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
        [Tooltip("The players that can select this. Leave null to allow for anyone to select.")]
        public Allegiance allegiance;

        [Tooltip("The name of the selection group this is in. Only selectable objects in the same group can be selected at the same time.")]
        [field: SerializeField] public string selectionGroup { get; } = "Default";

        [Tooltip("Called when this has been selected. Can be invoked to select this object")]
        public UnityEvent onSelected;

        [Tooltip("Called when this has been deselected. Can be invoked to deselect this object.")]
        public UnityEvent onDeselected;



        // Whether or not this is currently selected.
        public bool isSelected { get; private set; } = false;


        /// <summary>
        /// Handles is selected.
        /// </summary>
        private void Start()
        {
            onSelected.AddListener(delegate { isSelected = true; });
            onDeselected.AddListener(delegate { isSelected = false; });
        }
    }
}
