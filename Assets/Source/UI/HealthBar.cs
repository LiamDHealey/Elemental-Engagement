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
            canvasGroup = GetComponent<CanvasGroup>();
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
        public void FadeIn()
        {
            StartCoroutine(FadeInCoroutine());
        }

        /// <summary>
        /// Coroutine to perform a *fancy* fade in animation
        /// </summary>
        /// <returns></returns>
        IEnumerator FadeInCoroutine()
        {
            canvasGroup.alpha = 0;
            for (int i = 0; i < 10; i++)
            {
                canvasGroup.alpha += 0.1f;
                yield return new WaitForSeconds(0.01f);
            }
        }

        /// <summary>
        /// Fades the health slider out if visible.
        /// </summary>
        public void FadeOut()
        {
            StartCoroutine(FadeOutCoroutine());
        }

        /// <summary>
        /// Coroutine to perform a *fancy* fade in animation
        /// </summary>
        /// <returns></returns>
        IEnumerator FadeOutCoroutine()
        {
            canvasGroup.alpha = 1;
            for (int i = 0; i < 10; i++)
            {
                canvasGroup.alpha -= 0.1f;
                yield return new WaitForSeconds(0.01f);
            }
        }
    } 
}
