using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HideOnGroupSelect : MonoBehaviour
{
    private SelectionInputHandler input;
    private Image image;
    private AbilityInputHandler ability;

    void Start()
    {
        image = gameObject.GetComponent<Image>();
        input = GetComponentInParent<SelectionInputHandler>();
        ability = GetComponentInParent<AbilityInputHandler>();

        input.groupSelectionStarted.AddListener(HideCursor);
        input.groupSelectionStopped.AddListener(ShowCursor);
        ability.onAbilitySelected.AddListener(HideCursor);
        ability.onAbilityDeselected.AddListener(ShowCursor);

    }

    private void HideCursor()
    {
        image.enabled = false;
    }

    private void ShowCursor()
    {
        image.enabled = true;
    }
}
