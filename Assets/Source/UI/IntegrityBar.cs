using ElementalEngagement.Combat;
using ElementalEngagement.Favor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ElementalEngagement.UI
{
    /// <summary>
    /// Class for showing how much health something has.
    /// </summary>
    public class IntegrityBar : MonoBehaviour
    {
        [Tooltip("The health to display the integrity percent for.")]
        [SerializeField] private SacrificeLocation sacrificeLocation;

        [Tooltip("The distance from min or max value for an overlay to be enabled.")] [Min(0f)]
        [SerializeField] private float outOfRangeTolerance = 0.01f;

        [Tooltip("The object to enable when integrity is too low")]
        [SerializeField] private GameObject tooLowOverlay;

        [Tooltip("The object to enable when integrity is too high")]
        [SerializeField] private GameObject tooHighOverlay;

        [Tooltip("The slider to display the integrity on.")]
        [SerializeField] private Slider slider;

        /// <summary>
        /// Updates slider.
        /// </summary>
        private void Update()
        {
            slider.minValue = 0;
            slider.maxValue = sacrificeLocation.RequiredCapturePoints;
            slider.value = sacrificeLocation.integrity;
            tooLowOverlay.SetActive(sacrificeLocation.integrity <= outOfRangeTolerance);
            tooHighOverlay.SetActive(sacrificeLocation.integrity >= sacrificeLocation.RequiredCapturePoints - outOfRangeTolerance);
        }
    } 
}
