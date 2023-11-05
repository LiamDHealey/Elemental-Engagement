using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ElementalEngagement.Player
{
    /// <summary>
    /// Pans this game object.
    /// </summary>
    public class PanInputHandler : MonoBehaviour
    {
        [Tooltip("The input to get pan input from.")]
        [SerializeField] private PlayerInput input;

        [Tooltip("The speed multiplier for panning.")]
        [SerializeField] private float panspeed = 1f;

        /// <summary>
        /// Move this
        /// </summary>
        public void Pan(Vector2 input)
        {
            Vector2 delta = input * panspeed * Time.deltaTime;
            transform.position += new Vector3(delta.x, 0, delta.y);
        }
    }
}
