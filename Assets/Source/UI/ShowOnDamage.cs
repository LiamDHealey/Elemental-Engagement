using ElementalEngagement.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowOnDamage : MonoBehaviour
{
    public float flashTime = 1f;

    private float timeSinceDamage = 99999999;
    private Image[] images;

    // Start is called before the first frame update
    void Start()
    {
        GetComponentInParent<Health>().onDamaged.AddListener(delegate { timeSinceDamage = 0; });
        images = GetComponentsInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceDamage += Time.deltaTime;
        foreach (var image in images)
        {
            image.color = new Color(image.color.r, image.color.b, image.color.g, 1 - timeSinceDamage / flashTime);
        }
    }
}
