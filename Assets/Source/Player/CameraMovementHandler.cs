using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace ElementalEngagement.Player
{
    /// <summary>
    /// Pans & Zooms this game object.
    /// </summary>
    public class CameraMovementHandler : MonoBehaviour
    {
        [Tooltip("The input to get pan input from.")]
        [SerializeField] private PlayerInput input;

        [Tooltip("The speed multiplier for panning.")]
        [SerializeField] private float panSpeed = 1f;

        [Tooltip("The normalized acceleration this when starting or stopping panning.")]
        [SerializeField] private float panAcceleration = 3f;

        [Tooltip("The speed multiplier for zooming.")]
        [SerializeField] private float zoomSpeed = 1f;

        [Tooltip("The closest on the Y axis this object can zoom.")]
        [SerializeField] private float zoomY = 10f;

        [Tooltip("The multiplier for the deceleration")]
        public float decelerationBoost = 2f;

        private Vector3 minBound => CameraBounds.boundsBox.bounds.min;
        private Vector3 maxBound => CameraBounds.boundsBox.bounds.max;
        private float speedMultiplier;
        bool lastAccelerating = false;

        Vector3? startPos;

        /// <summary>
        /// Move this
        /// </summary>
        public void Pan(Vector2 input)
        {
            bool currentAccelerating = input.sqrMagnitude < 0.1;
            if (!lastAccelerating && currentAccelerating)
            {
                speedMultiplier = 0;
            }
            else
            {
                speedMultiplier = currentAccelerating
                    ? Mathf.Clamp01(speedMultiplier - panAcceleration * decelerationBoost * Time.deltaTime)
                    : Mathf.Clamp01(speedMultiplier + panAcceleration * Time.deltaTime);
            }

            lastAccelerating = currentAccelerating;

            Vector2 delta = input * panSpeed * speedMultiplier * Time.deltaTime;

            Vector3 pos = transform.position;
            transform.position = new Vector3(
                Mathf.Clamp(pos.x + delta.x, minBound.x, maxBound.x), 
                pos.y,
                Mathf.Clamp(pos.z + delta.y, minBound.z, maxBound.z));
        }

        /// <summary>
        /// Zoom this
        /// </summary>
        public void Zoom(Vector2 input)
        {
            Vector2 delta = input * zoomSpeed * Time.deltaTime;
            startPos ??= transform.position;

            transform.position = new Vector3(
                transform.position.x,
                Mathf.Clamp(transform.position.y + delta.y, zoomY, startPos.Value.y),
                transform.position.z
                );
        }
    }
}
