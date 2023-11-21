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

        [Tooltip("The camera boundary for the Grasslands map")]
        [SerializeField] private Vector3 map1Bounds;
        [Tooltip("The camera boundary for the Desert map")]
        [SerializeField] private Vector3 map2Bounds;
        [Tooltip("The camera boundary for the Tundra map")]
        [SerializeField] private Vector3 map3Bounds;

        [SerializeField] private Vector3 bounds;

        [SerializeField] Vector3 startPos;

        private void Start()
        {
            UnityEngine.SceneManagement.Scene currentMap = SceneManager.GetActiveScene();
            if (currentMap.name == "GrasslandMap")
            {
                bounds = map1Bounds;
            }
            else if (currentMap.name == "DesertMap")
            {
                bounds = map2Bounds;
            }
            else if (currentMap.name == "TundraMap")
            {
                bounds = map3Bounds;
            }
        }

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
