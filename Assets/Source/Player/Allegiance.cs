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
        /// <param name="this"></param>
        /// <param name="targetAllegiance"></param>
        /// <returns>true if aligned, false otherwise</returns>
        public bool CheckAnyAllegiance(Allegiance targetAllegiance)
        {
            return (this != null && targetAllegiance != null &&
                (this.faction != Faction.Unaligned && this.faction == targetAllegiance.faction ||
                this.god != Favor.MinorGod.Unaligned && this.god == targetAllegiance.god));
        }

        /// <summary>
        /// Returns true if the two units are aligned with both faction AND God
        /// </summary>
        /// <param name="this"></param>
        /// <param name="targetAllegiance"></param>
        /// <returns>true if aligned, false if not</returns>
        public bool CheckBothAllegiance(Allegiance targetAllegiance)
        {
            return (this != null && targetAllegiance != null &&
                (this.faction != Faction.Unaligned && this.faction == targetAllegiance.faction &&
                this.god != Favor.MinorGod.Unaligned && this.god == targetAllegiance.god));
        }
    }
}
