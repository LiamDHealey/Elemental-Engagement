using ElementalEngagement.Favor;
using ElementalEngagement.Player;
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
        [SerializeField] private AbilityIcon abilityIconPrefab;

        [Tooltip("The manager this is showing the abilities for.")]
        [SerializeField] private AbilityManager manager;

        // A list of all the ability icons.
        private List<AbilityIcon> abilityIcons =  new List<AbilityIcon>();

        /// <summary>
        /// Activates the selected overlay for the ability icon at ability index, and deactivates it for all other abilities.
        /// </summary>
        /// <param name="abilityIndex"> The index of the ability in FavorManager.unlockedAbilities. </param>
        public void SetSelectedAbility(int abilityIndex)
        {
            for (int i = 0; i < abilityIcons.Count; i++)
            {
                abilityIcons[i].selectedOverlayEnabled = abilityIndex == i;
            }
        }

        /// <summary>
        /// Initializes all of the ability icons.
        /// </summary>
        private void Start()
        {
            UpdateIcons();

            void UpdateIcons()
            {
                System.Collections.ObjectModel.ReadOnlyCollection<Combat.Ability> unlockedAbilities = FavorManager.unlockedAbilities;


                for (int i = 0; i < abilityIcons.Count; i++)
                {
                    abilityIcons[i].ability = unlockedAbilities[i];
                }

                for (int i = abilityIcons.Count; i < unlockedAbilities.Count; i++)
                {
                    AbilityIcon newIcon = Instantiate(abilityIconPrefab.gameObject).GetComponent<AbilityIcon>();
                    abilityIcons.Add(newIcon);
                    newIcon.transform.SetParent(transform);
                    newIcon.ability = unlockedAbilities[i];
                }

                for (int i = unlockedAbilities.Count; i < abilityIcons.Count; i++)
                {
                    Destroy(abilityIcons[i].gameObject);
                    abilityIcons.RemoveAt(i);
                }
            }
        }
    }
}
