using ElementalEngagement.Combat;
using ElementalEngagement.Favor;
using ElementalEngagement.Player;
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
        /// Initialize the onFavorChanged event
        /// </summary>
        private void Start()
        {
            if (onFavorChanged != null)
            {
                onFavorChanged = new UnityEvent<float>();
            }
        }

        private void Update()
        {
            Slider slider = GetComponent<Slider>();
        }
    }
}