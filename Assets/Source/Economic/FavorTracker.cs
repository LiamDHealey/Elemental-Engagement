using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ElementalEngagement.Economic
{
    /// <summary>
    /// Tracks the favor of all minor god for all player.
    /// </summary>
    public class FavorTracker : MonoBehaviour
    {
        // Stores the favor each god shows towards each player faction.
        public static IReadOnlyDictionary<Player.Faction, IReadOnlyDictionary<MinorGod, float>> factionToFavor { get { throw new System.NotImplementedException(); } }

        /// <summary>
        /// Adds an amount to the favor this god has towards a player.
        /// </summary>
        /// <param name="allegiance"> The faction to add favor for. </param>
        /// <param name="god"> The god to add favor for. </param>
        /// <param name="deltaFavor"> The amount of favor to add. </param>
        public static void ModifyFavor(Player.Faction allegiance, MinorGod god, float deltaFavor)
        {
            throw new System.NotImplementedException();
        }
    }
}
