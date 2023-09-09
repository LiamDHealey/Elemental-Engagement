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
        public Faction faction;

        /// <summary>
        /// Whether or not this is friendly towards another game object.
        /// </summary>
        /// <param name="other"> The other game object. </param>
        /// <returns> True if this has the same alignment as the other object. </returns>
        public bool IsAligned(GameObject other)
        {
            throw new System.NotImplementedException();
        }
    }
}
