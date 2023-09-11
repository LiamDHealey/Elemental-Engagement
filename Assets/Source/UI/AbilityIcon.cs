using ElementalEngagement.Combat;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ElementalEngagement.UI
{
    public class AbilityIcon : MonoBehaviour
    {
        [Tooltip("The image used to display the ability icon.")]
        [SerializeField] private Image iconImage;



        [Tooltip("Invoke to enable selected overlay.")]
        public UnityEvent onEnableSelectedOverlay;

        [Tooltip("Invoke to disable selected overlay.")]
        public UnityEvent onDisableSelectedOverlay;

        [Tooltip("Invoke to enable cooldown overlay.")]
        public UnityEvent onEnableCooldownOverlay;

        [Tooltip("Invoke to disable cooldown overlay.")]
        public UnityEvent onDisableCooldownOverlay;

        [Tooltip("Called every tick with the new cooldown percent passed in.")]
        [SerializeField] private UnityEvent<float> onCooldownPercentChanged;


        // The ability this is rending the icon for.
        public Ability ability
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }
    }
}