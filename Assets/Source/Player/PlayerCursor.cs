using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ElementalEngagement
{
    public class PlayerCursor : MonoBehaviour
    {
        [Tooltip("The input component that this will get mouse data from.")]
        [SerializeField] private PlayerInput input;

        [Tooltip("The camera from which to make the selection.")]
        [SerializeField] private new Camera camera;

        [Tooltip("The camera from which to make the selection.")]
        [SerializeField] private RectTransform gamepadCursor;

        /// <summary>
        /// Gets the screen to world ray under the cursor.
        /// </summary>
        /// <param name="ray"> The ray that when cast will get the object under the cursor. </param>
        /// <returns> True if the mouse position is vaild, false otherwise. </returns>
        public bool RayUnderCursor(out Ray ray)
        {
            if (input.actions["CursorPosition"].IsInProgress())
            {
                gamepadCursor.gameObject.SetActive(false);

                Vector2 cursorPosition = input.actions["CursorPosition"].ReadValue<Vector2>();

                if (!camera.pixelRect.Contains(cursorPosition))
                {
                    ray = new Ray();
                    return false;
                }

                ray = camera.ScreenPointToRay(cursorPosition);
                return true;
            }
            else
            {
                gamepadCursor.gameObject.SetActive(true);
                ray = new Ray(camera.transform.position, camera.transform.forward);
                return true;
            }
        }
    }
}