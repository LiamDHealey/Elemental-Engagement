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
    public class SelectionManager : MonoBehaviour
    {
        [Tooltip("The radius of the circular selection.")]
        [SerializeField] private float circularSelectionRadius = 10;

        [Tooltip("The input component that this will get mouse data from.")]
        [SerializeField] private PlayerInput input;

        [Tooltip("The allegiance use to determining what is selectable.")]
        [SerializeField] private Allegiance allegiance;

        [Tooltip("The cursor used to get the selection location from.")]
        [SerializeField] private PlayerCursor cursor;

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
        /// Bind Controls
        /// </summary>
        private void Start()
        {
            circularSelectionIndicator.SetActive(false);
            circularDeselectionIndicator.SetActive(false);

            input.actions["Select"].performed += Select;
            input.actions["CircularSelect"].performed += CircularSelectionStarted;
            input.actions["SelectAll"].performed += SelectAll;
            input.actions["DeselectAll"].performed += DeselectAll;
            input.actions["IssueCommand"].performed += IssueCommand;
            input.actions["IssueAltCommand"].performed += IssueAltCommand;
        }

        /// <summary>
        /// Unbinds controls
        /// </summary>
        private void OnDestroy()
        {
            input.actions["Select"].performed -= Select;
            input.actions["CircularSelect"].performed -= CircularSelectionStarted;
            input.actions["SelectAll"].performed -= SelectAll;
            input.actions["DeselectAll"].performed -= DeselectAll;
            input.actions["IssueCommand"].performed -= IssueCommand;
            input.actions["IssueAltCommand"].performed -= IssueAltCommand;
        }


        /// <summary>
        /// Selects the unit under the cursor.
        /// </summary>
        /// <param name="context"> The context of the selection input. </param>
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
            
            throw new NotImplementedException();
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


        /// <summary>
        /// Issues a command to all selected units.
        /// </summary>
        /// <param name="context"> The context of the command input. </param>
        public void IssueCommand()
        {
            if (!cursor.RayUnderCursor(out Ray screenToWorldRay))
                return;
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

                chosenReceiver?.ExecuteCommand(hit);
            }
        }




        /// <summary>
        /// Issues a alternate version of the command to all selected units.
        /// </summary>
        /// <param name="context"> The context of the command input. </param>
        private void IssueAltCommand(CallbackContext context)
        {
            throw new NotImplementedException();
        }






        /// <summary>
        /// Gets the selectable unit under the ray cursor.
        /// </summary>
        /// <param name="selectables"> The units that was under the ray. </param>
        /// <returns> True if there was a valid unit to select. </returns>
        private bool GetSelectableUnderCursor(out IEnumerable<Selectable> selectables, float radius, Transform hitLocationIndicator = null)
        {
            selectables = null;


            if (!cursor.RayUnderCursor(out Ray screenToWorldRay))
                return false;


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
            if (!cursor.RayUnderCursor(out Ray screenToWorldRay))
            {
                selectable = null;
                return false;
            }

            bool result = Physics.Raycast(screenToWorldRay, out RaycastHit hit, 9999f, selectableMask);

            selectable = hit.collider?.GetComponent<Selectable>();


            if (!hit.collider?.GetComponent<Allegiance>()?.CheckAnyAllegiance(allegiance) ?? true)
                return false;

            return result && selectable != null;
        }
    }
}