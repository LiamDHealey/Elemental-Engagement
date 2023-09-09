using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ElementalEngagement.Economic
{
    /// <summary>
    /// Tracks the favor of single minor god for all player.
    /// </summary>
    public class MinorGod : MonoBehaviour
    {
        // Stores the favor this god shows towards each player faction.
        public IReadOnlyDictionary<Player.Faction, float> FactionToFavor { get { throw new System.NotImplementedException(); } }

        /// <summary>
        /// Adds an amount to the favor this god has towards a player.
        /// </summary>
        /// <param name="allegiance"> The faction to add favor for. </param>
        /// <param name="deltaFavor"> The amount of favor to add. </param>
        public void ModifyFavor(Player.Faction allegiance, float deltaFavor)
        {
            throw new System.NotImplementedException();
        }
    }
}
