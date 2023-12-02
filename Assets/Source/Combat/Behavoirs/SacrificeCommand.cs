using ElementalEngagement.Favor;
using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace ElementalEngagement.Combat
{
    /// <summary>
    /// Allows this to sacrifice itself at a sacrifice location.
    /// </summary>
    [RequireComponent(typeof(Selectable))]
    public class SacrificeCommand : CommandReceiver
    {
        [Tooltip("The agent used to move this.")]
        [SerializeField] private Movement movement;

        [Tooltip("The allegiance of this. Can not be null.")]
        [SerializeField] private Allegiance allegiance;

        [Tooltip("The amount of favor gained by sacrificing 1 hp from this unit.")]
        [SerializeField] private float sacrificeHpValue;

        [Tooltip("The range that a sacrifice location needs to overlap in order to be sacrificed to by this.")]
        [SerializeField] private BindableCollider sacrificeRange;

        [Tooltip("Called when this starts sacrificing itself")]
        public UnityEvent onSacrificeBegin;

        [Tooltip("Called when this stops sacrificing itself")]
        public UnityEvent onSacrificeEnd;


        // The last sacrifice location this was commanded to sacrifice to.
        private Collider targetSacrificeLocation;


        /// <summary>
        /// Tests if an sacrifice command is followable.
        /// </summary>
        /// <param name="hitUnderCursor"> The hit result from under the cursor. </param>
        /// <returns> True if the destination is a sacrifice location, and is aligned with this. </returns>
        public override bool CanExecuteCommand(RaycastHit hitUnderCursor)
        {
            if (hitUnderCursor.collider == null)
                return false;
            if (hitUnderCursor.collider.GetComponent<SacrificeLocation>() == null)
                return false;

            return true;
        }

        /// <summary>
        /// Moves to the sacrifice location and then sacrifices itself to it by calling Sacrifice on the sacrifice location.
        /// </summary>
        /// <param name="hitUnderCursor"> The hit result from under the cursor. </param>
        /// <param name="selectedObjects"> The other selected objects. </param>
        /// <param name="isAltCommand"> Whether or not this should execute the alternate version of this command (if it exists). </param>
        public override void ExecuteCommand(RaycastHit hitUnderCursor, ReadOnlyCollection<Selectable> selectedObjects, bool isAltCommand)
        {
            commandInProgress = true;
            movement.SetDestination(this, SelectDestination(hitUnderCursor, selectedObjects));

            targetSacrificeLocation = hitUnderCursor.collider;
            if (sacrificeRange.overlappingColliders.Contains(targetSacrificeLocation))
                StartSacrificing(targetSacrificeLocation);

            sacrificeRange.onTriggerEnter.AddListener(StartSacrificing);
            sacrificeRange.onTriggerExit.AddListener(StopSacrificing);
        }

        /// <summary>
        /// Cancels any command this is currently performing.
        /// </summary>
        public override void CancelCommand()
        {
            commandInProgress = false;

            if (targetSacrificeLocation != null)
                StopSacrificing(targetSacrificeLocation);

            sacrificeRange.onTriggerEnter.RemoveListener(StartSacrificing);
            sacrificeRange.onTriggerExit.RemoveListener(StopSacrificing);
        }


        /// <summary>
        /// Causes this to start sacrificing itself to the sacrifice location.
        /// </summary>
        /// <param name="collider"> The sacrifice location that has entered the sacrifice range. </param>
        private void StartSacrificing(Collider collider)
        {
            if (collider != targetSacrificeLocation)
                return;

            movement.PreventMovement(this);
            targetSacrificeLocation.GetComponent<SacrificeLocation>().StartSacrificing(this);
        }

        /// <summary>
        /// Causes this to stop sacrificing itself to the sacrifice location.
        /// </summary>
        /// <param name="collider"> The sacrifice location that has left the sacrifice range. </param>
        private void StopSacrificing(Collider collider)
        {
            if (collider != targetSacrificeLocation)
                return;

            movement.AllowMovement(this);
            onSacrificeEnd?.Invoke();
            targetSacrificeLocation.GetComponent<SacrificeLocation>().StopSacrificing(this);
        }

        protected override List<Vector3> GetDestinations(RaycastHit hit, ReadOnlyCollection<Selectable> selectedObjects, float unitWidth = -10f)
        {
            List<Vector3> destinations = new List<Vector3>(selectedObjects.Count);
            Vector3 averageLocation = Vector3.zero;
            foreach (Selectable obj in selectedObjects)
            {
                averageLocation += obj.transform.position;
            }
            averageLocation /= selectedObjects.Count;
            Vector3 direction = (averageLocation - hit.point).normalized;

            for (int i = 0; i < selectedObjects.Count; i++)
            {
                destinations.Add(hit.collider.bounds.center + Quaternion.AngleAxis(i * 360 / selectedObjects.Count, Vector3.up) * direction * (hit.collider.bounds.extents.x));
            }

            return destinations;
        }
    }
}