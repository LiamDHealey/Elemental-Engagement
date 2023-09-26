using ElementalEngagement.Favor;
using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private NavMeshAgent agent;

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
            if (!hitUnderCursor.collider.GetComponent<Allegiance>()?.CheckBothAllegiance(allegiance) ?? false)
                return false;

            return true;
        }

        /// <summary>
        /// Moves to the sacrifice location and then sacrifices itself to it by calling Sacrifice on the sacrifice location.
        /// </summary>
        /// <param name="hitUnderCursor"> The hit result from under the cursor. </param>
        public override void ExecuteCommand(RaycastHit hitUnderCursor)
        {
            agent.isStopped = false;
            commandInProgress = true;
            agent.SetDestination(hitUnderCursor.collider.transform.position);

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

            agent.isStopped = true;
            onSacrificeBegin?.Invoke();
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

            agent.isStopped = false;
            onSacrificeEnd?.Invoke();
            targetSacrificeLocation.GetComponent<SacrificeLocation>().StopSacrificing(this);
        }
    }
}