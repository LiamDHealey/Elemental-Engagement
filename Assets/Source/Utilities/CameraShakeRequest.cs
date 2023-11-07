using ElementalEngagement.Utilities;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraShakeRequest : MonoBehaviour
{
    public bool shouldShake = false;
    public float elapsedTime = 5f;
    public bool hasShaken = false;
    

    void Update()
    {
        CameraShake.isShaking = shouldShake;

        if (shouldShake)
            hasShaken = true;

        if (hasShaken)
            elapsedTime += Time.deltaTime;

        if (elapsedTime > 5)
            shouldShake = false;
    }

    void OnDestroy()
    {
        CameraShake.isShaking = false;
        hasShaken = false;
    }
    //public bool isShaking   { set => CameraShake.isShaking = isTrue; }

}
