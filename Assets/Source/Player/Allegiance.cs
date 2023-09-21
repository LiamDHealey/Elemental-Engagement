using ElementalEngagement.Favor;
using UnityEngine;

namespace ElementalEngagement.Player
{
    /// <summary>
    /// Aligns an object with a player.
    /// </summary>
    public class Allegiance : MonoBehaviour
    {
        // The player this is aligned with.
        public Faction faction;

        // The god this is aligned with.
        public MinorGod god;

        /// <summary>
        /// Returns true if the two units are aligned with either faction or God
        /// </summary>
        /// <param name="selfAllegiance"></param>
        /// <param name="targetAllegiance"></param>
        /// <returns>true if aligned, false otherwise</returns>
        public static bool CheckAnyAllegiance(Allegiance selfAllegiance, Allegiance targetAllegiance)
        {
            return (selfAllegiance != null && targetAllegiance != null &&
                (selfAllegiance.faction != Faction.Unaligned && selfAllegiance.faction == targetAllegiance.faction ||
                selfAllegiance.god != Favor.MinorGod.Unaligned && selfAllegiance.god == targetAllegiance.god));
        }

        /// <summary>
        /// Returns true if the two units are aligned with both faction AND God
        /// </summary>
        /// <param name="selfAllegiance"></param>
        /// <param name="targetAllegiance"></param>
        /// <returns>true if aligned, false if not</returns>
        public static bool CheckBothAllegiance(Allegiance selfAllegiance, Allegiance targetAllegiance)
        {
            return (selfAllegiance != null && targetAllegiance != null &&
                (selfAllegiance.faction != Faction.Unaligned && selfAllegiance.faction == targetAllegiance.faction &&
                selfAllegiance.god != Favor.MinorGod.Unaligned && selfAllegiance.god == targetAllegiance.god));
        }
    }
}
