using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace ElementalEngagement.UI
{
    public class TooltipDisplay : MonoBehaviour
    {
        public TMP_Text textBox;
        public Image icon;

        private AbilityInputHandler handler;

        // Start is called before the first frame update
        void Start()
        {
            handler = GetComponentInParent<AbilityInputHandler>();
            handler.onAbilitySelected.AddListener(ShowTooltip);
            handler.onAbilityDeselected.AddListener(() => gameObject.SetActive(false));
            gameObject.SetActive(false);
        }

        private void ShowTooltip()
        {
            gameObject.SetActive(true);
            if (textBox != null)
                textBox.text = handler.selectedAbility.toolTip;

            if (icon != null)
                icon.sprite = handler.selectedAbility.icon;
        }
    } 
}
