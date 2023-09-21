using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElementalEngagement.Utilities
{
    /// <summary>
    /// Exposes certain functionality to the animator.
    /// </summary>
    public class Destroyable : MonoBehaviour
    {
        /// <summary>
        /// Destroys this component's game object
        /// </summary>
        public void DestroySelf()
        {
            Destroy(this);
        }

        /// <summary>
        /// Destroys the parent game object of this.
        /// </summary>
        public void DestroyParent()
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }
}