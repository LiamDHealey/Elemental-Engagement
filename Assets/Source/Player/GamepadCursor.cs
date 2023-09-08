using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ElementalEngagement.Player
{
    /// <summary>
    /// Handles a tracking, updating, & displaying of a virtual mouse for gamepad users.
    /// </summary>
    public class GamepadCursor : MonoBehaviour
    {
        [Tooltip("The input component that this will feed mouse data to.")]
        [SerializeField] private PlayerInput input;

        [Tooltip("The transform of the cursor visuals.")]
        [SerializeField] private RectTransform cursorTransform;

        [Tooltip("The canvas the cursor is being rendered on.")]
        [SerializeField] private Canvas cursorCanvas;

        [Tooltip("The speed of the cursor in pixels/s.")]
        [SerializeField] private float cursorSpeed = 1000f;


        // Useful tutorial: https://www.youtube.com/watch?v=Y3WNwl1ObC8
    }
}