using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ElementalEngagement.Combat;
using UnityEditor.ShaderKeywordFilter;

public class SpriteFlasher : MonoBehaviour
{
    public List<SpriteRenderer> renderers;
    public Color color;
    public Color healColor;
    public float flashLength = .25f;
    private float remainingFlashTime = 0;

    private List<Color> originalColors = new List<Color>();
    private void Start()
    {
        for (int i = 0; i < renderers.Count; i++)
        {
            originalColors.Add(renderers[i].color);
        }
    }

    public void Flash(Damage damage)
    {
        if (damage.amount > 0)
        {
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


    public void FlashWithoutDamage()
    {
        foreach (SpriteRenderer renderer in renderers)
        {
            renderer.color = color;
        }

        remainingFlashTime = flashLength;
    }
    private void Update()
    {
        if (remainingFlashTime <= 0)
        {
            for (int i = 0; i < renderers.Count; i++)
            {
                renderers[i].color = originalColors[i];
            }
        }
        else
        {
            remainingFlashTime -= Time.deltaTime;
        }
    }
}
