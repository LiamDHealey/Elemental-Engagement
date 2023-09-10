using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Events;

namespace ElementalEngagement.Economic
{
    /// <summary>
    /// Tracks the favor of all minor god for all player.
    /// </summary>
    public class FavorManager : MonoBehaviour
    {
        [Tooltip("How each god's favor unlocks things.")]
        [SerializeField] private FavorProgressionSettings _progressionSettings;
        public static ReadOnlyDictionary<MinorGod, FavorProgressionSettings.GodProgressionSettings> progressionSettings { get { throw new System.NotImplementedException(); } }


        // Stores the favor each god shows towards each player faction.
        public static ReadOnlyDictionary<Player.Faction, IReadOnlyDictionary<MinorGod, float>> factionToFavor { get { throw new System.NotImplementedException(); } }

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
