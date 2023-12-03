using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{

    public Color color;
    public float flashLength = .25f;
    private float remainingFlashTime = 0;

    public void Flash()
    {
        GetComponent<SpriteRenderer>().color = color;

        remainingFlashTime = flashLength;

    }

    private void Update()
    {
        if (remainingFlashTime <= 0)
            GetComponent<SpriteRenderer>().color = Color.white;
        else
            remainingFlashTime -= Time.deltaTime;
    }
}
