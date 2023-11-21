using ElementalEngagement.Favor;
using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ElementalEngagement.UI
{
    public class TierIndicator : MonoBehaviour
    {
        [SerializeField] int tier;
        [SerializeField] TMP_Text text;
        [SerializeField] List<Image> images;
        [SerializeField] List<Sprite> tierSprites;
        
        
        AbilityInputHandler abilityHandler;

        private void Start()
        {
            abilityHandler = GetComponentInParent<AbilityInputHandler>();

            int max = FavorManager.progressionSettings.abilitiesPerTier[tier];
            foreach (Image image in images)
            {
                image.enabled = false;
            }

            abilityHandler.onAbilityUnlocked.AddListener(
                delegate
                {
                    if (abilityHandler.abilitiesInTiers.Count <= tier)
                        return;

                    int current = abilityHandler.abilitiesInTiers[tier];
                    text.text = $"{current}/{max}";
                    foreach (Image image in images)
                    {
                        image.enabled = true;
                        image.sprite = tierSprites[current - 1];
                    }
                });
        }
    }
}