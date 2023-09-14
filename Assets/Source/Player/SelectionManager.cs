using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using System.Linq;
using System.Collections.ObjectModel;

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
        [Tooltip("The input component that this will get mouse data from.")]
        [SerializeField] private PlayerInput input;

        [Tooltip("The cursor used to get the selection location from.")]
        [SerializeField] private PlayerCursor cursor;


        // All of the currently selected objects.
        private List<Selectable> _selectedObjects = new List<Selectable>();
        public ReadOnlyCollection<Selectable> selectedObjects { get => _selectedObjects.AsReadOnly(); }

        /// <summary>
        /// Bind Controls
        /// </summary>
        private void Start()
        {
            input.actions["Select"].started += SelectionStarted;
            input.actions["IssueCommand"].performed += IssueCommand;
        }

        /// <summary>
        /// Unbinds controls
        /// </summary>
        private void OnDestroy()
        {
            input.actions["Select"].started -= SelectionStarted;
            input.actions["IssueCommand"].performed -= IssueCommand;
        }

        /// <summary>
        /// Begins selecting of units.
        /// </summary>
        /// <param name="context"> The context of the selection input. </param>
        private void SelectionStarted(CallbackContext context)
        {
            StartCoroutine(UpdateSelection());
        }

        /// <summary>
        /// Updates the selection of units under the cursor while the Select input is in progress.
        /// </summary>
        /// <returns> The time to wait between selection updates. </returns>
        private IEnumerator UpdateSelection()
        {
            int newSelectionCount = 0;

            // Add to selection.
            while (input.actions["Select"].inProgress)
            {
                yield return null;

                if (!cursor.RayUnderCursor(out Ray screenToWorldRay))
                    continue;

                if (GetSelectableUnderCursor(screenToWorldRay, out Selectable selectable))
                {
                    if (!selectable.isSelected)
                    {
                        _selectedObjects.Add(selectable);
                        newSelectionCount++;
                    }
                    selectable.isSelected = true;
                }
            }


            // Clear selection.
            if (newSelectionCount == 0)
            {
                if (!cursor.RayUnderCursor(out Ray screenToWorldRay))
                    yield break;

                // Clear just targeted unit
                if (GetSelectableUnderCursor(screenToWorldRay, out Selectable selectable))
                {
                    _selectedObjects.Remove(selectable);
                    selectable.isSelected = false;
                }
                // Clear whole selection
                else
                {
                    IEnumerable<Selectable> selectedObjects = new List<Selectable>(_selectedObjects);
                    foreach (Selectable selectedObject in selectedObjects)
                    {
                        _selectedObjects.Remove(selectedObject);
                        selectedObject.isSelected = false;
                    }
                }
            }



            bool GetSelectableUnderCursor(Ray screenToWorldRay, out Selectable selectable)
            {
                bool result = Physics.Raycast(screenToWorldRay, out RaycastHit hit);
                selectable = hit.collider?.GetComponent<Selectable>();
                return result && selectable != null;
            }
        }


        /// <summary>
        /// Issues a command to all selected units.
        /// </summary>
        /// <param name="context"> The context of the command input. </param>
        private void IssueCommand(CallbackContext context)
        {
            if (!cursor.RayUnderCursor(out Ray screenToWorldRay))
                return;
            bool result = Physics.Raycast(screenToWorldRay, out RaycastHit hit);


            foreach (Selectable selectable in selectedObjects)
            {
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
    }
}