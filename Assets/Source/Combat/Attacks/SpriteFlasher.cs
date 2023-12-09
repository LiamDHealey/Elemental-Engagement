using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpriteFlasher : MonoBehaviour
{
    public List<SpriteRenderer> renderers;
    public Color color;
    public float flashLength = .25f;
    private float remainingFlashTime = 0;

    public void Flash()
    {
        foreach (SpriteRenderer renderer in renderers)
        {
            renderer.color = new Color(1, 0, 0, 0.5f);
        }

        remainingFlashTime = flashLength;

    }

    private void Update()
    {
        if (remainingFlashTime <= 0)
        {
            foreach (SpriteRenderer renderer in renderers)
            {
                renderer.color = Color.white;
            }
        }
        else
        {
            remainingFlashTime -= Time.deltaTime;
        }
    }
}
