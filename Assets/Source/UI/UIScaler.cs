using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScaler : MonoBehaviour
{
    Vector3 originalScale;
    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;
    }

    private void Update()
    {
        transform.localScale = originalScale * Settings.uiScale;
    }
}
