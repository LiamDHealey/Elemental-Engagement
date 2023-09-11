using ElementalEngagement.Favor;
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
        // The player this is aligned with.
        public Faction faction;

        // The god this is aligned with.
        public MinorGod god;
    }
}
