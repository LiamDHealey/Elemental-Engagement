using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElementalEngagement.Player
{
    /// <summary>
    /// Aligns an object with a player.
    /// </summary>
    public class Allegiance : MonoBehaviour
    {
        public Alignment alignment;

        /// <summary>
        /// Whether or not this is friendly towards another game object.
        /// </summary>
        /// <param name="other"> The other game object. </param>
        /// <returns> True if this has the same alignment as the other object. </returns>
        public bool IsAligned(GameObject other)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Whether or not this is aligned with one of the given flags.
        /// </summary>
        /// <param name="other"> The flags to test. </param>
        /// <returns> True if this component's alignment is contained in the given flags. </returns>
        public bool IsAligned(AlignmentFlags other)
        {
            throw new System.NotImplementedException();
        }

        public enum Alignment
        {
            Unaligned,
            PlayerOne,
            PlayerTwo,
        }

        [System.Flags]
        public enum AlignmentFlags
        {
            PlayerOne,
            PlayerTwo,
        }
    }
}
