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
        /// Sets the fill color of this bar.
        /// </summary>
        /// <param name="color"> Color formatted as a string: "r, b, g, a" where rgba values are floats 0-1. </param>
        public void SetColor(string color)
        {
            string[] rgba = color.Split(", ");

            slider.fillRect.GetComponent<Image>().color = new Color(float.Parse(rgba[0]), float.Parse(rgba[1]), float.Parse(rgba[2]), float.Parse(rgba[3]));
        }

        /// <summary>
        /// Sets the fill color of this bar.
        /// </summary>
        /// <param name="color"> The color of the bar. </param>
        public void SetColor(Color color)
        {
            slider.fillRect.GetComponent<Image>().color = color;
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
    } 
}
