using ElementalEngagement.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ElementalEngagement.UI
{
    /// <summary>
    /// Class for showing how much health something has.
    /// </summary>
    public class HealthBar : MonoBehaviour
    {
        [Tooltip("The health to display the hp percent for.")]
        [SerializeField] private Health healthToDisplay;

        [Tooltip("The slider to display the health on.")]
        [SerializeField] Slider slider;

        /// <summary>
        /// When spawned, fade the health slider out
        /// </summary>
        private void Start()
        {
            FadeOut();
        }

        /// <summary>
        /// Updates slider.
        /// </summary>
        private void Update()
        {
            slider.minValue = 0;
            slider.maxValue = healthToDisplay.maxHp;
            slider.value = healthToDisplay.hp;
        }

        /// <summary>
        /// Fades the health slider in if not visible.
        /// </summary>
        private void FadeIn()
        {
            slider.gameObject.SetActive(true);
        }

        /// <summary>
        /// Fades the health slider out if visible.
        /// </summary>
        private void FadeOut()
        {
            slider.gameObject.SetActive(false);
        }
    } 
}
