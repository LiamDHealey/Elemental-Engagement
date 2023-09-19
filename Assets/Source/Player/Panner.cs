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
        [SerializeField] private float panspeed = 1f;

        /// <summary>
        /// Move this
        /// </summary>
        private void Update()
        {
            Vector2 delta = input.actions["Pan"].ReadValue<Vector2>() * panspeed * Time.deltaTime;
            transform.position += new Vector3(delta.x, 0, delta.y);
        }
    }
}
