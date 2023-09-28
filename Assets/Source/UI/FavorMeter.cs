using ElementalEngagement.Combat;
using ElementalEngagement.Favor;
using ElementalEngagement.Player;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ElementalEngagement.UI
{
    /// <summary>
    /// Displays how much favor a player has for a given god.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class FavorMeter : MonoBehaviour
    {
        [Tooltip("The god who's favor should be displayed.")]
        [SerializeField] private Favor.MinorGod god;

        [Tooltip("The allegiance used to determine which player this displays the favor for.")]
        [SerializeField] private Allegiance allegiance;

        [Tooltip("Called whenever the favor this meter is tracking changes.")]
        [SerializeField] private UnityEvent<float> onFavorChanged;

        /// <summary>
        /// Create listener for the onFavorChanged event.
        /// This listener will invoke the event if the god and the faction align with
        /// the current player's faction and god.
        /// </summary>
        private void Start()
        {
            FavorManager.onFavorChanged.AddListener((faction,god)=>
            {
                if (god == this.god && faction == allegiance.faction)
                {
                    Tuple<Faction,MinorGod> favorUpdateTuple = new Tuple<Faction,MinorGod>(faction, god);
                    float favorUpdate = FavorManager.factionToFavor[favorUpdateTuple];
                    onFavorChanged.Invoke(favorUpdate);
                }
            });
        }
    }
}