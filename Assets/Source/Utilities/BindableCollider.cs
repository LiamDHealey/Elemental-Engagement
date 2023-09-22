using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace ElementalEngagement
{
    /// <summary>
    /// Allows on trigger enter and exit events to be bound to.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class BindableCollider : MonoBehaviour
    {
        // Called when on trigger enter is called.
        public UnityEvent<Collider> onTriggerEnter;
        // Called when on trigger exit is called.
        public UnityEvent<Collider> onTriggerExit;
        // Called when on trigger enter is called.
        public UnityEvent<Collision> onCollisionEnter;
        // Called when on trigger exit is called.
        public UnityEvent<Collision> onCollisionExit;


        // Contains all of the currently overlapping colliders in the order they entered this trigger.
        private List<Collider> _overlappingColliders = new List<Collider>();
        public ReadOnlyCollection<Collider> overlappingColliders => new ReadOnlyCollection<Collider>(_overlappingColliders);

        /// <summary>
        /// Invokes event and tracks overlapping colliders.
        /// </summary>
        /// <param name="other"> THe collider now being overlapped. </param>
        private void OnTriggerEnter(Collider other)
        {
            _overlappingColliders.Add(other);
            onTriggerEnter?.Invoke(other);
        }

        /// <summary>
        /// Invokes event and tracks overlapping colliders.
        /// </summary>
        /// <param name="other"> THe collider no longer being overlapped. </param>
        private void OnTriggerExit(Collider other)
        {
            _overlappingColliders.Remove(other);
            onTriggerExit?.Invoke(other);
        }

        /// <summary>
        /// Invokes event and tracks overlapping colliders.
        /// </summary>
        /// <param name="other"> THe collider now being overlapped. </param>
        private void OnCollisionEnter(Collision collision)
        {
            _overlappingColliders.Add(collision.collider);
            onCollisionEnter?.Invoke(collision);
        }

        /// <summary>
        /// Invokes event and tracks overlapping colliders.
        /// </summary>
        /// <param name="other"> THe collider no longer being overlapped. </param>
        private void OnCollisionExit(Collision collision)
        {
            _overlappingColliders.Remove(collision.collider);
            onCollisionExit?.Invoke(collision);
        }
    }
}