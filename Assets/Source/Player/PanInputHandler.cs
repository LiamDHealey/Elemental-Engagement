using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

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

        [SerializeField] private Vector2 bounds;

        [SerializeField] Vector3 startPos;

        /// <summary>
        /// Move this
        /// </summary>
        public void Pan(Vector2 input)
        {
            Vector2 delta = input * panspeed * Time.deltaTime;

            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x + delta.x, startPos.x - bounds.x, startPos.x + bounds.x), 
                transform.position.y,
                Mathf.Clamp(transform.position.z + delta.y, startPos.z - bounds.y, startPos.z + bounds.y)
                );
        }
    }
}
