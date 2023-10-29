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



        [Header("Unlocking/Locking")]

        [Tooltip("Invoked when once when this is permanently locked.")]
        [SerializeField] private MinimizableEvent onLocked;

        [Tooltip("Invoked when once when this is permanently unlocked.")]
        [SerializeField] private MinimizableEvent onUnlocked;


        [Header("Selection")]

        [Tooltip("Invoked when this ability is selected.")]
        [SerializeField] private MinimizableEvent onSelected;

        [Tooltip("Invoked when when this ability is deselected.")]
        [SerializeField] private MinimizableEvent onDeselected;


        [Header("Cooldowns")]

        [Tooltip("Invoked when the cooldown has begun.")]
        [SerializeField] private MinimizableEvent onCooldownBegan;

        [Tooltip("Invoked when the cooldown ended.")]
        [SerializeField] private MinimizableEvent onCooldownEnded;

        [Tooltip("Called every tick with the new cooldown percent passed in.")]
        [SerializeField] private MinimizableEvent<float> onCooldownPercentChanged;



        [Header("God")]

        [Tooltip("Invoked on start when this is an icon for the water god.")]
        [SerializeField] private MinimizableEvent onStart_WaterGod;

        [Tooltip("Invoked on start when this is an icon for the fire god.")]
        [SerializeField] private MinimizableEvent onStart_FireGod;

        [Tooltip("Invoked on start when this is an icon for the earth god.")]
        [SerializeField] private MinimizableEvent onStart_EarthGod;


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
                //if (_selectedOverlayEnabled == value)
                //    return;

                //_selectedOverlayEnabled = value;

                //if (_selectedOverlayEnabled)
                //{
                //    onEnableSelectedOverlay?.Invoke();
                //}
                //else
                //{
                //    onDisableSelectedOverlay?.Invoke();
                //}
            }
        }

        // Set this to enable/disable the selected overlay
        private bool _cooldownOverlayEnabled = false;
        private bool cooldownOverlayEnabled
        {
            get => _cooldownOverlayEnabled;
            set
            {
                //if (_cooldownOverlayEnabled == value)
                //    return;

                //_cooldownOverlayEnabled = value;

                //if (_cooldownOverlayEnabled)
                //{
                //    onEnableCooldownOverlay?.Invoke();
                //}
                //else
                //{
                //    onDisableCooldownOverlay?.Invoke();
                //}
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

        [System.Serializable]
        private class MinimizableEvent<T>
        {
            public UnityEvent<T> _;
            public void Invoke(T value) => _?.Invoke(value);
        }

        [System.Serializable]
        private class MinimizableEvent
        {
            public UnityEvent _;
            public void Invoke() => _?.Invoke();
        }
    }
}