using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ElementalEngagement
{
    public static class MathHelpers
    {
        /// <summary>
        /// Gets where a ray intersects the ground plane.
        /// </summary>
        /// <param name="ray"> The ray to intersect with the ground. </param>
        /// <returns> The location the ray hits the ground at. </returns>
        public static Vector3 IntersectWithGround(Ray ray)
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            if (!plane.Raycast(ray, out float enter))
                throw new System.Exception("Ray did not intersect ground plane.");
            return ray.origin + ray.direction * enter;
        }
    }
}