using ElementalEngagement.Favor;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace ElementalEngagement.Player
{
    /// <summary>
    /// Aligns an object with a player.
    /// </summary>
    public class Allegiance : MonoBehaviour
    {
        // The player this is aligned with.
        [SerializeField] private Faction _faction;
        public Faction faction
        {
            get => _faction;
            set
            {
                if (_faction != value)
                {
                    _faction = value; 
                    InvokeOnFactionChanged();
                }
            }
        }

        // The god this is aligned with.
        public MinorGod god;

        // Called when this allegiance's faction was set.
        public List<FactionEvent> onFactionChanged = new List<FactionEvent> { new FactionEvent(Faction.Unaligned), new FactionEvent(Faction.PlayerOne), new FactionEvent(Faction.PlayerTwo) };


        private void OnValidate()
        {
            InvokeOnFactionChanged();
        }

        private void Start()
        {
            InvokeOnFactionChanged();
        }

        private void InvokeOnFactionChanged()
        {
            onFactionChanged.First(fe => fe.faction == faction).onSelected?.Invoke();
        }

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


        [System.Serializable]
        public class FactionEvent
        {
            [ViewOnly] [Tooltip("The Faction this is tied to.")]
            [SerializeField] public Faction faction;

            [Tooltip("Called when the faction has been set to this.")]
            public UnityEvent onSelected;

            public FactionEvent(Faction faction)
            {
                this.faction = faction;
            }
        }
    }
}
