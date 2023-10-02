using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace ElementalEngagement.Utilities
{
    /// <summary>
    /// Allows control over the color of a sprite via unity events
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteColor : MonoBehaviour
    {
        public void SetColor(string color)
        {
            string[] rgba = color.Split(", ");

            GetComponent<SpriteRenderer>().color = new Color(float.Parse(rgba[0]), float.Parse(rgba[1]), float.Parse(rgba[2]), float.Parse(rgba[3]));
        }
    }

}