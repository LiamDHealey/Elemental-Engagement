using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flipper : MonoBehaviour
{
    public RectTransform[] xFlippedElements;
    public RectTransform[] yFlippedElements;

    public void SetFlipped(bool newFlipped)
    {
        foreach (RectTransform flippedElement in xFlippedElements)
        {
            flippedElement.localScale = new Vector3(newFlipped ? -1 : 1, flippedElement.localScale.y);
        }
        foreach (RectTransform flippedElement in yFlippedElements)
        {
            flippedElement.localScale = new Vector3(flippedElement.localScale.x, newFlipped ? -1 : 1);
        }
    }
}
