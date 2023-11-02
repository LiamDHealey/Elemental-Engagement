using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using System.Linq;
using System.Collections.ObjectModel;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;
using UnityEditor;

namespace ElementalEngagement.Player
{
    /// <summary>
    /// Handles the selection, commanding, & deselection of selectable objects. 
    /// 
    /// Can select either by clicking on an individual selectable 
    /// or by clicking and dragging to create a selection box which will select all members of the selection group with the most member within the box.
    /// 
    /// Once objects have been selected further clicks will cancel all current commands on the selected object, then executes the command with the highest priority.
    /// See also: CommandReciever
    /// </summary>
    [RequireComponent(typeof(Allegiance))]
    public class SelectionInputHandler : MonoBehaviour
    {
        [Tooltip("The radius of the circular selection.")]
        [SerializeField] private float circularSelectionRadius = 10;

        [Tooltip("The input component that this will get mouse data from.")]
        [SerializeField] private PlayerInput input;

        [Tooltip("The allegiance use to determining what is selectable.")]
        [SerializeField] private Allegiance allegiance;

        [Tooltip("The game object to spawn at the selection location")]
        [SerializeField] private GameObject circularSelectionIndicator;

        [Tooltip("The game object to spawn at the deselection location")]
        [SerializeField] private GameObject circularDeselectionIndicator;

        [Tooltip("The mask to use when detecting selectable objects.")]
        [SerializeField] private LayerMask selectableMask;

        [Tooltip("The mask to use when placing a circular selection.")]
        [SerializeField] private LayerMask circularMask;

        [Tooltip("The mask to use when detecting locations where commands can be issued.")]
        [SerializeField] private LayerMask commandMask;

        // All of the currently selected objects.
        private List<Selectable> _selectedObjects = new List<Selectable>();
        public ReadOnlyCollection<Selectable> selectedObjects { get => _selectedObjects.AsReadOnly(); }


        /// <summary>
        /// Selects the unit under the cursor.
        /// </summary>
        public void Select()
        {
            if (GetSelectableUnderCursor(out Selectable selectable) && !selectable.isSelected)
            {
                _selectedObjects.Add(selectable);
                selectable.isSelected = true;
            }
        }

        /// <summary>
        /// Begins selecting of units.
        /// </summary>
        /// <param name="isInProgress"> Will evaluate to true while the circular selection is in progress. </param>
        public void StartCircularSelection(Func<bool> isInProgress)
        {
            DeselectAll();
            StartCoroutine(UpdateSelection());

            /// <summary>
            /// Updates the selection of units under the cursor while the Select input is in progress.
            /// </summary>
            /// <returns> The time to wait between selection updates. </returns>
            IEnumerator UpdateSelection()
            {
                // Add to selection.
                while (isInProgress())
                {
                    if (GetSelectableUnderCursor(out IEnumerable<Selectable> selectables, circularSelectionRadius, circularSelectionIndicator.transform))
                    {
                        foreach (Selectable selectable in selectables)
                        {
                            if (selectable.isSelected)
                                continue;

                            _selectedObjects.Add(selectable);
                            selectable.isSelected = true;
                        }
                    }

                    yield return null;
                }
                circularSelectionIndicator.SetActive(false);
            }
        }


        /// <summary>
        /// Select all units of the selected types on screen.
        /// </summary>
        /// <param name="context"> The context of the selection input. </param>
        public void SelectAll()
        {
            Ray corner1 = GetComponent<Camera>().ScreenPointToRay(new Vector2(GetComponent<Camera>().pixelRect.height, 0));
            Debug.DrawRay(corner1.origin, corner1.direction * 1000);
            Ray corner2 = GetComponent<Camera>().ScreenPointToRay(new Vector2(0, GetComponent<Camera>().pixelRect.width));
            Debug.DrawRay(corner2.origin, corner2.direction * 1000);

            Vector3 corner1GroundPos = MathHelpers.IntersectWithGround(corner1);
            Vector3 corner2GroundPos = MathHelpers.IntersectWithGround(corner2);

            Rect intersectBox = new Rect(corner1GroundPos.x, corner1GroundPos.z, corner1GroundPos.x - corner2GroundPos.x, corner2GroundPos.z - corner1GroundPos.z);
            Debug.Log(intersectBox);


            Collider[] hitColliders = Physics.OverlapBox(intersectBox.center, intersectBox.size / 2);

            if (hitColliders.Length > 0)
            {
                Debug.Log("Got HitColliders: " + hitColliders.Length);
            }

            String currentSelectedTag = "";

            if (GetSelectableUnderCursor(out Selectable selectable))
            {
                currentSelectedTag = selectable.tag;
            }

            Debug.Log("currentSelectTag: " + currentSelectedTag);

            DeselectAll();
            
            foreach (Collider collider in hitColliders)
            {
                Debug.Log("Getting each collider name: " + collider.name);

                //Selectable colliderSelect = collider.GetComponent<Selectable>();

                //Debug.Log("Getting each collider tag: " + colliderSelect.tag);

                //if (colliderSelect.tag == currentSelectedTag)
                //{

                //    _selectedObjects.Add(colliderSelect);
                //    colliderSelect.isSelected = true;
                //}
            }
        }


        /// <summary>
        /// Deselects all units.
        /// </summary>
        /// <param name="context"> The context of the selection input. </param>
        public void DeselectAll()
        {
            IEnumerable<Selectable> selectedObjects = new List<Selectable>(_selectedObjects);
            foreach (Selectable selectedObject in selectedObjects)
            {
                _selectedObjects.Remove(selectedObject);
                selectedObject.isSelected = false;
            }
        }

        public void IssueCommand(bool isAltCommand)
        {
            Ray screenToWorldRay = new Ray(transform.position, transform.forward);
            bool result = Physics.Raycast(screenToWorldRay, out RaycastHit hit, 9999f, commandMask);


            foreach (Selectable selectable in selectedObjects)
            {
                if (selectable == null)
                    continue;

                CommandReceiver[] receivers = selectable.GetComponents<CommandReceiver>();
                foreach (CommandReceiver receiver in receivers)
                    receiver.CancelCommand();

                // Get highest priority receiver that can execute this command.
                CommandReceiver chosenReceiver = receivers
                    .Where(receiver => receiver.CanExecuteCommand(hit))
                    .OrderByDescending(receiver => receiver.commandPriority)
                    .FirstOrDefault();

                chosenReceiver?.ExecuteCommand(hit, isAltCommand);
            }
        }




        /// <summary>
        /// Gets the selectable unit under the ray cursor.
        /// </summary>
        /// <param name="selectables"> The units that was under the ray. </param>
        /// <returns> True if there was a valid unit to select. </returns>
        private bool GetSelectableUnderCursor(out IEnumerable<Selectable> selectables, float radius, Transform hitLocationIndicator = null)
        {
            selectables = null;


            Ray screenToWorldRay = new Ray(transform.position, transform.forward);


            bool result = Physics.Raycast(screenToWorldRay, out RaycastHit hit, 9999f, circularMask);

            if (hitLocationIndicator != null)
            {
                if (result)
                {
                    circularSelectionIndicator.SetActive(true);
                    hitLocationIndicator.position = hit.point;
                }
                else
                {
                    circularSelectionIndicator.SetActive(false);
                }    
            }

            if (!result)
                return false;

            Debug.DrawLine(hit.point, hit.point + new Vector3(radius, 0, 0), Color.red, 10f);  
            selectables = Physics.OverlapSphere(hit.point, radius)
                .Select(collider => collider.GetComponent<Selectable>()).NotNull()
                .Where(selectable => selectable.GetComponent<Allegiance>()?.CheckAnyAllegiance(allegiance) ?? true);

            return selectables.Count() > 0;
        }

        /// <summary>
        /// Gets the selectable unit under the ray cursor.
        /// </summary>
        /// <param name="selectable"> The unit that was under the ray. </param>
        /// <returns> True if there was a valid unit to select. </returns>
        private bool GetSelectableUnderCursor(out Selectable selectable)
        {
            Ray screenToWorldRay = new Ray(transform.position, transform.forward);

            bool result = Physics.Raycast(screenToWorldRay, out RaycastHit hit, 9999f, selectableMask);

            selectable = hit.collider?.GetComponent<Selectable>();


            if (!hit.collider?.GetComponent<Allegiance>()?.CheckAnyAllegiance(allegiance) ?? true)
                return false;

            return result && selectable != null;
        }
    }
}