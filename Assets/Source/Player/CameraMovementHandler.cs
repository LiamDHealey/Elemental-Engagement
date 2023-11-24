using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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

        [Tooltip("The speed multiplier for zooming.")]
        [SerializeField] private float zoomspeed = 1f;

        [Tooltip("The camera boundary for the Grasslands map. Y-axis is for zoom height. The bigger the Y-axis, the farther one can zoom out.")]
        [SerializeField] private Vector3 map1PositiveBounds;
        [Tooltip("The camera boundary for the Grasslands map.")]
        [SerializeField] private Vector3 map1NegativeBounds;

        [Tooltip("The camera boundary for the Desert map. Y-axis is for zoom height. The bigger the Y-axis, the farther one can zoom out.")]
        [SerializeField] private Vector3 map2PositiveBounds;
        [Tooltip("The camera boundary for the Desert map.")]
        [SerializeField] private Vector3 map2NegativeBounds;

        [Tooltip("The camera boundary for the Tundra map. Y-axis is for zoom height. The bigger the Y-axis, the farther one can zoom out.")]
        [SerializeField] private Vector3 map3PositiveBounds;
        [Tooltip("The camera boundary for the Tundra map.")]
        [SerializeField] private Vector3 map3NegativeBounds;

        private Vector3 boundsPositive;
        private Vector3 boundsNegative;

        [SerializeField] Vector3 startPos;

        private void Start()
        {
            UnityEngine.SceneManagement.Scene currentMap = SceneManager.GetActiveScene();
            if (currentMap.name == "GrasslandMap")
            {
                boundsPositive = map1PositiveBounds;
                boundsNegative = map1NegativeBounds;
            }
            else if (currentMap.name == "DesertMap")
            {
                boundsPositive = map2PositiveBounds;
                boundsNegative = map2NegativeBounds;
            }
            else if (currentMap.name == "TundraMap")
            {
                boundsPositive = map3PositiveBounds;
                boundsNegative = map3NegativeBounds;
            }
        }

        /// <summary>
        /// Move this
        /// </summary>
        public void Pan(Vector2 input)
        {
            Vector2 delta = input * panspeed * Time.deltaTime;

            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x + delta.x, startPos.x - boundsNegative.x, startPos.x + boundsPositive.x), 
                transform.position.y,
                Mathf.Clamp(transform.position.z + delta.y, startPos.z - boundsNegative.z, (startPos.z + boundsPositive.z)-30)
                );
        }

        /// <summary>
        /// Zoom this
        /// </summary>
        public void Zoom(Vector2 input)
        {
            Vector2 delta = input * zoomspeed * Time.deltaTime;

            transform.position = new Vector3(
                transform.position.x,
                Mathf.Clamp(transform.position.y + delta.y, startPos.y, startPos.y + boundsPositive.y),
                transform.position.z
                );
        }
    }
}
