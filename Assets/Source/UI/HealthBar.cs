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

        //CanvasGroup used for fading the health bar in and out
        CanvasGroup canvasGroup;

        /// <summary>
        /// When spawned, fade the health slider out and instantiate the canvasGroup
        /// </summary>
        private void Start()
        {
            bool isShrine = (gameObject.transform.parent.name == "Player1FireShrine") || (gameObject.transform.parent.name == "Player1WaterShrine") || (gameObject.transform.parent.name == "Player1EarthShrine") || (gameObject.transform.parent.name == "Player2FireShrine") || (gameObject.transform.parent.name == "Player2WaterShrine") || (gameObject.transform.parent.name == "Player2EarthShrine");
            bool isHumanArmy = (gameObject.transform.parent.name == "HumanArmy(Clone)");
            canvasGroup = GetComponent<CanvasGroup>();
            if (!isShrine && !isHumanArmy)
            {
                FastFadeOut();
            }
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
        public void FadeIn()
        {
            canvasGroup.alpha = 1.0f;
        }

        /// <summary>
        /// Quickly fades the health slider out if visible.
        /// </summary>
        public void FastFadeOut()
        {
            canvasGroup.alpha = 0;
        }
    } 
}
