using ElementalEngagement.Combat;
using ElementalEngagement.Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ElementalEngagement.UI
{
    /// <summary>
    /// Used to display the icon of a single ability. Can also display it as being selected and on cooldown.
    /// </summary>
    public class AbilityIcon : MonoBehaviour
    {
        [Tooltip("The image used to display the ability icon.")]
        [SerializeField] private Image iconImage;
            
        [Tooltip("The manager this is showing the ability for.")]
        public AbilityManager manager;



        [Tooltip("Invoked when the selected overlay is enabled.")]
        public UnityEvent onEnableSelectedOverlay;

        [Tooltip("Invoked when the selected overlay is disabled.")]
        public UnityEvent onDisableSelectedOverlay;

        [Tooltip("Invoked when the cooldown overlay is enabled.")]
        public UnityEvent onEnableCooldownOverlay;

        [Tooltip("Invoked when the cooldown overlay is disabled.")]
        public UnityEvent onDisableCooldownOverlay;

        [Tooltip("Called every tick with the new cooldown percent passed in.")]
        [SerializeField] private UnityEvent<float> onCooldownPercentChanged;


        // The ability this is rending the icon for.
        private Ability _ability;
        public Ability ability
        {
            get => _ability;
            set
            {
                if (value == _ability)
                    return;
                _ability = value;
                iconImage.sprite = _ability.icon;
            }
        }

        // Set this to enable/disable the selected overlay
        private bool _selectedOverlayEnabled = false;
        public bool selectedOverlayEnabled
        {
            get => _selectedOverlayEnabled;
            set
            {
                if (_selectedOverlayEnabled == value)
                    return;

                _selectedOverlayEnabled = value;

                if (_selectedOverlayEnabled)
                {
                    onEnableSelectedOverlay?.Invoke();
                }
                else
                {
                    onDisableSelectedOverlay?.Invoke();
                }
            }
        }

        // Set this to enable/disable the selected overlay
        private bool _cooldownOverlayEnabled = false;
        private bool cooldownOverlayEnabled
        {
            get => _cooldownOverlayEnabled;
            set
            {
                if (_cooldownOverlayEnabled == value)
                    return;

                _cooldownOverlayEnabled = value;

                if (_cooldownOverlayEnabled)
                {
                    onEnableCooldownOverlay?.Invoke();
                }
                else
                {
                    onDisableCooldownOverlay?.Invoke();
                }
            }
        }


        private void Update()
        {
            if(!manager.abilityCooldowns.TryGetValue(ability, out float currentCooldown))
                currentCooldown = 0;

            cooldownOverlayEnabled = currentCooldown != 0;
            if (cooldownOverlayEnabled)
                onCooldownPercentChanged?.Invoke(currentCooldown/ability.cooldown);
        }
    }
}