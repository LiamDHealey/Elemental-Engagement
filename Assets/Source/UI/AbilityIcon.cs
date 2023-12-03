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
    [ExecuteAlways]
    public class AbilityIcon : MonoBehaviour
    {
        [Tooltip("The ability this is the icon for.")]
        [SerializeField] private Ability _ability;
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

        [Tooltip("The image used to display the ability icon.")]
        [SerializeField] private Image iconImage;
            
        [Tooltip("The manager this is showing the ability for.")]
        public AbilityInputHandler manager;



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

        [Tooltip("Called every tick with the new cooldown percent passed in.")]
        [SerializeField] private MinimizableEvent<string> onCooldownTimeChanged;



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
                    onSelected?.Invoke();
                }
                else
                {
                    onDeselected?.Invoke();
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
                    onCooldownBegan?.Invoke();
                }
                else
                {
                    onCooldownEnded?.Invoke();
                }
            }
        }
        private void Start()
        {
            if (!Application.IsPlaying(gameObject))
                return;

            manager = GetComponentInParent<AbilityInputHandler>();

            manager.onSelectedAbilityChanged.AddListener(ability => { (ability == this.ability ? onSelected : onDeselected)?.Invoke(); });

            if (manager.IsAbilityLocked(ability))
            {
                onLocked?.Invoke();
            }
            else if (manager.IsAbilityUnlocked(ability))
            {
                onUnlocked?.Invoke();
            }
            else
            {
                manager.onAbilityLocked.AddListener(ability => { if (ability == this.ability) onLocked?.Invoke(); });
                manager.onAbilityUnlocked.AddListener(ability => { if (ability == this.ability) onUnlocked?.Invoke(); });
            }
        }

        private void Update()
        {
            iconImage.sprite = ability?.icon ?? null;
            if (ability == null) return;

            float currentCooldown = 0;
            if ((!manager?.abilityCooldowns.TryGetValue(ability, out currentCooldown)) ?? false)
                currentCooldown = 0;

            cooldownOverlayEnabled = currentCooldown != 0;
            if (cooldownOverlayEnabled)
            {
                onCooldownPercentChanged?.Invoke(currentCooldown / ability.cooldown);
                onCooldownTimeChanged?.Invoke(currentCooldown.ToString("N0"));
            }

            selectedOverlayEnabled = manager?.selectedAbility == ability;
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