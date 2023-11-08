using ElementalEngagement.Utilities;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraShakeRequest : MonoBehaviour
{
    public bool shouldShake = false;
    public float elapsedTime = 0;
    

    void Update()
    {
        CameraShake.isShaking = shouldShake;

        elapsedTime += Time.deltaTime;

        if (elapsedTime > 5)
        {
            shouldShake = false;
            elapsedTime = 5f;
        }
    }

    void OnDestroy()
    {
        CameraShake.isShaking = false;
    }
    //public bool isShaking   { set => CameraShake.isShaking = isTrue; }

}
