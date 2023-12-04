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
        /// Updates slider.
        /// </summary>
        private void Update()
        {
            slider.minValue = 0;
            slider.maxValue = healthToDisplay.maxHp;
            slider.value = healthToDisplay.hp;
        }
    } 
}
