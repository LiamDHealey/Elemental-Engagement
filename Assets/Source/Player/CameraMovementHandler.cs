using System;
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
    public class CameraMovementHandler : MonoBehaviour
    {
        [Tooltip("The input to get pan input from.")]
        [SerializeField] private PlayerInput input;

        [Tooltip("The speed multiplier for panning.")]
        [SerializeField] private float panspeed = 1f;

        [SerializeField] private Vector3 bounds;

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
                Mathf.Clamp(transform.position.z + delta.y, startPos.z - bounds.z, startPos.z + bounds.z)
                );
        }

        /// <summary>
        /// Zoom this
        /// </summary>
        public void Zoom(Vector2 input)
        {
            Vector2 delta = input * panspeed * Time.deltaTime;

            transform.position = new Vector3(
                transform.position.x,
                Mathf.Clamp(transform.position.y + delta.y, startPos.y, startPos.y + bounds.y),
                transform.position.z
                );
        }
    }
}
