using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{

    public Color color;
    public float flashLength = 0.5f;
    private float remainingFlashTime = 0;
    private bool shouldFlash;

    public void Flash()
    {
        GetComponent<SpriteRenderer>().color = color;

        if (!shouldFlash)
        {
            remainingFlashTime = flashLength;
            shouldFlash = true;
        }
    }

    private void Update()
    {
        if (shouldFlash)
            Flash();

        if (remainingFlashTime <= 0)
        {
            GetComponent<SpriteRenderer>().color = Color.white;
            shouldFlash = false;
        }
        
        else
        {
            remainingFlashTime -= flashLength;
        }
    }
}
