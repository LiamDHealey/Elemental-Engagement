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
        [SerializeField] private GameObject[] playerOneOverlay;

        [Tooltip("The objects to enable when owned by player two")]
        [SerializeField] private GameObject[] playerTwoOverlay;

        [Tooltip("The objects to enable when gaining favor")]
        [SerializeField] private GameObject[] gainingFavorOverlay;

        [Tooltip("The slider to display the integrity on.")]
        [SerializeField] private Slider slider;

        private void Start()
        {
            Allegiance allegiance = sacrificeLocation.GetComponent<Allegiance>();
            sacrificeLocation.onClaimed.AddListener(UpdatePlayer);
            sacrificeLocation.onDecaptured.AddListener(UpdatePlayer);


            void UpdatePlayer()
            {
                switch (allegiance.faction)
                {
                    case Faction.Unaligned:
                        foreach(GameObject overlay in playerOneOverlay.Union(playerTwoOverlay))
                        {
                            overlay.SetActive(false);
                        }
                        break;


                    case Faction.PlayerOne:
                        foreach (GameObject overlay in playerOneOverlay)
                        {
                            overlay.SetActive(true);
                        }
                        foreach (GameObject overlay in playerTwoOverlay)
                        {
                            overlay.SetActive(false);
                        }
                        break;


                    case Faction.PlayerTwo:
                        foreach (GameObject overlay in playerTwoOverlay)
                        {
                            overlay.SetActive(true);
                        }
                        foreach (GameObject overlay in playerOneOverlay)
                        {
                            overlay.SetActive(false);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Updates slider.
        /// </summary>
        private void Update()
        {
            slider.minValue = 0;
            slider.maxValue = sacrificeLocation.state == SacrificeLocation.State.Capturing ? sacrificeLocation.requiredCapturePoints : sacrificeLocation.requiredDecapturePoints;
            slider.value = sacrificeLocation.capturePoints;

            bool gainingFavor = sacrificeLocation.IsGainingFavor();
            foreach (GameObject overlay in gainingFavorOverlay)
            {
                overlay.SetActive(gainingFavor);
            }
        }
    } 
}
