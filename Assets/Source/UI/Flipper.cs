using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flipper : MonoBehaviour
{
    public RectTransform[] flippedElements;

    public void SetFlipped(bool newFlipped)
    {
        foreach (RectTransform flippedElement in flippedElements)
        {
            flippedElement.localScale = new Vector3(newFlipped ? -1 : 1, 1);
        }
    }
}
