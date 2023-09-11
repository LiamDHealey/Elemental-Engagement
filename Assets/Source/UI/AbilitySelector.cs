using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElementalEngagement.UI
{
    /// <summary>
    /// Displays all unlocked abilities for the given player, and show which is selected and which are on cooldown.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class AbilitySelector : MonoBehaviour
    {
        [Tooltip("The prefab used to show unlocked abilities & whether they are selected or on cooldown.")]
        [SerializeField] private AbilityIcon abilityIconPrefab
            ;
        [Tooltip("The allegiance used to determine which player this shows the abilities for.")]
        [SerializeField] private Player.Allegiance allegiance;

        /// <summary>
        /// Activates the selected overlay for the ability icon at ability index, and deactivates it for all other abilities.
        /// </summary>
        /// <param name="abilityIndex"> The index of the ability in FavorManager.unlockedAbilities. </param>
        public void SetSelectedAbility(int abilityIndex)
        {
            throw new System.NotImplementedException();
        }


        /// <summary>
        /// Enables the cooldown overlay for the ability icon at ability index.
        /// </summary>
        /// <param name="abilityIndex"> The index of the ability in FavorManager.unlockedAbilities. </param>
        public void EnableCooldownOverlay(int abilityIndex)
        {
            throw new System.NotImplementedException();
        }


        /// <summary>
        /// Disables the cooldown overlay for the ability icon at ability index.
        /// </summary>
        /// <param name="abilityIndex"> The index of the ability in FavorManager.unlockedAbilities. </param>
        public void DisableCooldownOverlay(int abilityIndex)
        {
            throw new System.NotImplementedException();
        }
    }
}
