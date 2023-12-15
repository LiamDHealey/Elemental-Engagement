using ElementalEngagement.Combat;
using ElementalEngagement.Favor;
using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ElementalEngagement.UI
{
    /// <summary>
    /// Class for showing how much health something has.
    /// </summary>
    public class CaptureBar : MonoBehaviour
    {
        [Tooltip("The health to display the integrity percent for.")]
        [SerializeField] private SacrificeLocation sacrificeLocation;

        [Tooltip("The objects to enable when owned by player one")]
        [SerializeField] private GameObject playerOneOverlay;

        [Tooltip("The objects to enable when owned by player two")]
        [SerializeField] private GameObject playerTwoOverlay;

        [Tooltip("The slider to display player one's capture progress on.")]
        [SerializeField] private Slider playerOneCapturingSlider;

        [Tooltip("The slider to display player two's capture progress on.")]
        [SerializeField] private Slider playerTwoCapturingSlider;

        [Tooltip("The slider to display the remaining lockout time on")]
        [SerializeField] private Slider lockoutSlider;

        private void Start()
        {
            Allegiance allegiance = sacrificeLocation.GetComponent<Allegiance>();

            sacrificeLocation.onCaptured.AddListener(delegate
            {
                playerOneCapturingSlider.value = sacrificeLocation.requiredCapturePoints;
                playerTwoCapturingSlider.value = sacrificeLocation.requiredCapturePoints;
                playerOneOverlay.SetActive(allegiance.faction == Faction.PlayerOne);
                playerTwoOverlay.SetActive(allegiance.faction == Faction.PlayerTwo);
                playerOneCapturingSlider.gameObject.SetActive(allegiance.faction == Faction.PlayerOne);
                playerTwoCapturingSlider.gameObject.SetActive(allegiance.faction == Faction.PlayerTwo);
            });

            sacrificeLocation.onDecaptured.AddListener(delegate
            {
                playerOneCapturingSlider.value = 0;
                playerTwoCapturingSlider.value = 0;
                playerOneOverlay.SetActive(false);
                playerTwoOverlay.SetActive(false);
                playerOneCapturingSlider.gameObject.SetActive(true);
                playerTwoCapturingSlider.gameObject.SetActive(true);
            });
        }

        /// <summary>
        /// Updates slider.
        /// </summary>
        private void Update()
        {
            switch (sacrificeLocation.state)
            {
                case SacrificeLocation.State.Neutral:
                    lockoutSlider.maxValue = sacrificeLocation.neutralLockoutTime;
                    lockoutSlider.value = sacrificeLocation.remainingNeutralTime;
                    lockoutSlider.gameObject.SetActive(sacrificeLocation.remainingNeutralTime > 0);

                    playerOneCapturingSlider.maxValue = sacrificeLocation.requiredCapturePoints;
                    playerOneCapturingSlider.value = sacrificeLocation.capturePoints[Faction.PlayerOne];

                    playerTwoCapturingSlider.maxValue = sacrificeLocation.requiredCapturePoints;
                    playerTwoCapturingSlider.value = sacrificeLocation.capturePoints[Faction.PlayerTwo];
                    break;


                case SacrificeLocation.State.Capturing:
                    lockoutSlider.gameObject.SetActive(false);

                    playerOneCapturingSlider.maxValue = sacrificeLocation.requiredCapturePoints;
                    playerOneCapturingSlider.value = sacrificeLocation.capturePoints[Faction.PlayerOne];

                    playerTwoCapturingSlider.maxValue = sacrificeLocation.requiredCapturePoints;
                    playerTwoCapturingSlider.value = sacrificeLocation.capturePoints[Faction.PlayerTwo];
                    break;


                case SacrificeLocation.State.Decapturing:
                    lockoutSlider.gameObject.SetActive(false);

                    playerOneCapturingSlider.maxValue = sacrificeLocation.requiredDecapturePoints;
                    playerOneCapturingSlider.value = sacrificeLocation.requiredDecapturePoints - sacrificeLocation.decapturePoints;

                    playerTwoCapturingSlider.maxValue = sacrificeLocation.requiredDecapturePoints;
                    playerTwoCapturingSlider.value = sacrificeLocation.requiredDecapturePoints - sacrificeLocation.decapturePoints;
                    break;
            }
        }
    } 
}
