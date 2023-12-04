using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HideOnGroupSelect : MonoBehaviour
{
    SelectionInputHandler input;
    Image image;
    void Start()
    {
        image = gameObject.GetComponent<Image>();
        input = GetComponentInParent<SelectionInputHandler>();
        input.groupSelectionStarted.AddListener(HideCursor);
        input.groupSelectionStopped.AddListener(ShowCursor);
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
