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
using UnityEditor;
using ElementalEngagement.UI;
using ElementalEngagement.Combat;
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
    public class SelectionInputHandler : MonoBehaviour
    {
        [Tooltip("The radius of the circular selection.")]
        [SerializeField] private float circularSelectionRadius = 10;

        [Tooltip("The radius to select a single unit in. (Should be the same as the UI circle")]
        [SerializeField] private float regularSelectionRadius = 10;

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

        // Keeps track of the deselectWhenKilled actions attached to each selectable
        private Dictionary<Selectable, UnityAction> deselectWhenKilledTracker = new Dictionary<Selectable, UnityAction>();

        //Checks if the player can double-tap to select all similar units.
        private bool canSelectAll = false;

        private Camera camera;

        public UnityEvent groupSelectionStarted;
        public UnityEvent groupSelectionStopped;

        private bool selectedThisTick = false;

        private void Awake()
        {
            camera = GetComponent<Camera>();
        }

        private void Update()
        {
            selectedThisTick = false;
            if (GetSelectableUnderCursor(out Selectable selectable, regularSelectionRadius) && selectable.isSelected)
            {
                canSelectAll = true;
            } else
            {
                canSelectAll = false;
            }
        }
        /// <summary>
        /// Selects the unit under the cursor.
        /// </summary>
        public void Select()
        {
            if (GetSelectableUnderCursor(out Selectable selectable, regularSelectionRadius) && !selectable.isSelected)
            {
                if (selectable.isSelected)
                    return;
                _selectedObjects.Add(selectable);
                selectable.isSelected = true;
                addKillDelegate(selectable);
                selectedThisTick = true;
            }
        }

        /// <summary>
        /// Add the deselectWhenKilled Delegate when the object is selected.
        /// </summary>
        public void addKillDelegate(Selectable selectable)
        {
            if (!deselectWhenKilledTracker.ContainsKey(selectable))
            {
                UnityAction onKillAction = new UnityAction(delegate { deselectWhenKilled(selectable); });
                selectable.gameObject.GetComponent<Health>().onKilled.AddListener(onKillAction);
                deselectWhenKilledTracker.Add(selectable, onKillAction);
            }
        }

        /// <summary>
        /// When the selected unit is killed, remove it from the selectedObjects array.
        /// </summary>
        public void deselectWhenKilled(Selectable selectedObject)
        {
            _selectedObjects.Remove(selectedObject);
            selectedObject.isSelected = false;
        }

        /// <summary>
        /// Begins selecting of units.
        /// </summary>
        /// <param name="isInProgress"> Will evaluate to true while the circular selection is in progress. </param>
        public void StartCircularSelection(Func<bool> isInProgress)
        {
            //DeselectAll();
            StartCoroutine(UpdateSelection());

            /// <summary>
            /// Updates the selection of units under the cursor while the Select input is in progress.
            /// </summary>
            /// <returns> The time to wait between selection updates. </returns>
            IEnumerator UpdateSelection()
            {
                groupSelectionStarted?.Invoke();
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
                            addKillDelegate(selectable);
                            selectedThisTick = true;
                        }
                    }

                    yield return null;
                }
                circularSelectionIndicator.SetActive(false);
                groupSelectionStopped?.Invoke();
            }
        }


        /// <summary>
        /// Select all units of the selected types on screen.
        /// </summary>
        /// <param name="context"> The context of the selection input. </param>
        public void SelectAll()
        {
            if (canSelectAll)
            {
                Ray topCornerRay = camera.ScreenPointToRay(camera.pixelRect.size);
                Vector3 topCornerPos = MathHelpers.IntersectWithGround(topCornerRay);
                Ray bottomCornerRay = camera.ScreenPointToRay(Vector2.zero);
                Vector3 bottomCornerPos = MathHelpers.IntersectWithGround(bottomCornerRay);

                Vector3 centerGroundPos = MathHelpers.IntersectWithGround(new Ray(transform.position, transform.forward));

                Vector3 topExtent = (topCornerPos - centerGroundPos);
                Vector3 bottomExtent = (bottomCornerPos - centerGroundPos);
                Vector3 extent = new Vector3((Mathf.Abs(topExtent.x) + Mathf.Abs(bottomExtent.x)) / 2, 
                                             50, 
                                             (Mathf.Abs(topExtent.z) + Mathf.Abs(bottomExtent.z)) / 2);
                
                Collider[] hitColliders = Physics.OverlapBox(centerGroundPos, extent);

                if (GetSelectableUnderCursor(out Selectable selectable, regularSelectionRadius))
                {
                    string currentSelectedTag = selectable.tag;


                    foreach (Collider collider in hitColliders)
                    {
                        Allegiance colliderAllegiance = collider.GetComponent<Allegiance>();
                        if (collider.tag == currentSelectedTag && colliderAllegiance.faction == allegiance.faction)
                        {
                            Selectable colliderSelect = collider.GetComponent<Selectable>();
                            if (colliderSelect.isSelected)
                                continue;

                            _selectedObjects.Add(colliderSelect);
                            colliderSelect.isSelected = true;
                            addKillDelegate(colliderSelect);
                            selectedThisTick = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Selects every unit in the game.
        /// </summary>
        public void SelectEverything()
        {
            Selectable[] allSelectables = FindObjectsOfType<Selectable>();

            foreach (Selectable select in allSelectables)
            {
                Allegiance unitAllegiance = select.GetComponent<Allegiance>();

                if (!select.isSelected && unitAllegiance.CheckFactionAllegiance(allegiance))
                {
                    _selectedObjects.Add(select);
                    select.isSelected = true;
                    addKillDelegate(select);
                    selectedThisTick = true;
                }
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
                if (selectedObject == null)
                    continue;

                selectedObject.gameObject.GetComponent<Health>().onKilled.RemoveListener(deselectWhenKilledTracker[selectedObject]);
                selectedObject.isSelected = false;
            }
        }

        public void IssueCommand(bool isAltCommand)
        {
            if (!GetSelectableUnderCursor(out Selectable select, regularSelectionRadius))
            {
                Ray screenToWorldRay = new Ray(transform.position, transform.forward);
                if (!Physics.Raycast(screenToWorldRay, out RaycastHit hit, 9999f, commandMask))
                    return;

                if (selectedThisTick)
                    return;

                for (int i = 0; i < selectedObjects.Count; i++)
                {
                    if (_selectedObjects[i] == null)
                    {
                        _selectedObjects.RemoveAt(i);
                        i--;
                    }
                }
                foreach (Selectable selectable in selectedObjects)
                {
                    if (selectable == null)
                        continue;

                    CommandReceiver[] receivers = selectable.GetComponents<CommandReceiver>();
                    foreach (CommandReceiver receiver in receivers)
                    {
                        receiver.CancelCommand();
                    }

                    // Get highest priority receiver that can execute this command.
                    CommandReceiver chosenReceiver = receivers
                        .Where(receiver => receiver.CanExecuteCommand(hit))
                        .OrderByDescending(receiver => receiver.commandPriority)
                        .FirstOrDefault();

                    chosenReceiver?.ExecuteCommand(hit, selectedObjects, isAltCommand);
                }
            }
        }

        public CommandReceiver GetCurrentCommand()
        {
            Ray screenToWorldRay = new Ray(transform.position, transform.forward);
            if (!Physics.Raycast(screenToWorldRay, out RaycastHit hit, 9999f, commandMask))
                return null;

            return selectedObjects
                .Where(selectable => selectable != null)
                .SelectMany(selectable => selectable.GetComponents<CommandReceiver>())
                .Where(receiver => receiver.CanExecuteCommand(hit))
                .OrderByDescending(receiver => receiver.commandPriority)
                .FirstOrDefault();
        }

        /// <summary>
        /// Stop all selected units from performing their current command.
        /// </summary>
        public void StopSelectedCommands()
        {
            foreach (Selectable selectable in selectedObjects)
            {
                if (selectable == null)
                    continue;

                CommandReceiver[] receivers = selectable.GetComponents<CommandReceiver>();
                foreach (CommandReceiver receiver in receivers)
                {
                    receiver.CancelCommand();
                }
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
            Vector3 hit = MathHelpers.IntersectWithGround(screenToWorldRay);


            if (hitLocationIndicator != null)
            {
                hitLocationIndicator.gameObject.SetActive(true);
                hitLocationIndicator.position = hit;
            }

            selectables = Physics.OverlapSphere(hit, radius)
                .Select(collider => collider.GetComponent<Selectable>()).NotNull()
                .Where(selectable => selectable.GetComponent<Allegiance>()?.CheckAnyAllegiance(allegiance) ?? true);

            return selectables.Count() > 0;
        }

        /// <summary>
        /// Gets the selectable unit under the ray cursor.
        /// </summary>
        /// <param name="selectable"> The unit that was under the ray. </param>
        /// <returns> True if there was a valid unit to select. </returns>
        private bool GetSelectableUnderCursor(out Selectable selectable, float radius)
        {
            Ray screenToWorldRay = new Ray(transform.position, transform.forward);

            bool result = Physics.SphereCast(screenToWorldRay, radius, out RaycastHit hit, 99999f, selectableMask);

            selectable = hit.collider?.GetComponent<Selectable>();


            if (!hit.collider?.GetComponent<Allegiance>()?.CheckAnyAllegiance(allegiance) ?? true)
                return false;

            return result && selectable != null;
        }
    }
}