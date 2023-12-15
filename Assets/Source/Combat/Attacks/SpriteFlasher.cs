using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ElementalEngagement.Combat;

public class SpriteFlasher : MonoBehaviour
{
    public List<SpriteRenderer> renderers;
    public Color color;
    public Color healColor;
    public float flashLength = .25f;
    private float remainingFlashTime = 0;

    public void Flash(Damage damage)
    {
        if (damage.amount > 0)
        {
            Debug.Log(transform.parent);
            foreach (SpriteRenderer renderer in renderers)
            {
                renderer.color = color;
            }

            remainingFlashTime = flashLength;
        }
        if (damage.amount < 0)
        {
            Debug.Log(transform.parent);
            foreach (SpriteRenderer renderer in renderers)
            {
                renderer.color = healColor;
            }

            remainingFlashTime = flashLength;
        }
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
