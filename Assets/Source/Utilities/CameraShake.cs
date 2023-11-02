using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElementalEngagement.Utilities
{

    public class CameraShake : MonoBehaviour
    {
        public float intensity = 1f;
        public static bool isShaking = false;

        void Update()
        {
            if (isShaking)
            {
                Vector2 offset = Random.insideUnitCircle * intensity;
                transform.position += new Vector3(offset.x, 0, offset.y);
            }
        }
    }
}
