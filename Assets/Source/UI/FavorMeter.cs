using ElementalEngagement.Combat;
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
        [SerializeField] private Player.Allegiance allegiance;

        [Tooltip("Called whenever the favor this meter is tracking changes.")]
        [SerializeField] private UnityEvent<float> onFavorChanged;
    }
}