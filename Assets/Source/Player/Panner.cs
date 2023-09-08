using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ElementalEngagement.Player
{
    /// <summary>
    /// Pans this game object.
    /// </summary>
    public class Panner : MonoBehaviour
    {
        [Tooltip("The input to get pan input from.")]
        [SerializeField] private PlayerInput input;

        [Tooltip("The speed multiplier for panning.")]
        [SerializeField] private float Panspeed = 1f;
    }
}
