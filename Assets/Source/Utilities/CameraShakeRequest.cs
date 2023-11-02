using ElementalEngagement.Utilities;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraShakeRequest : MonoBehaviour
{
    public bool shouldShake = false;

    void Update()
    {
      CameraShake.isShaking = shouldShake;
    }
    //public bool isShaking   { set => CameraShake.isShaking = isTrue; }

}
