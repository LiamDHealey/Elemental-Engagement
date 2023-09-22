using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

/// <summary>
/// Allows control over the outline of a sprite
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class Outline : MonoBehaviour
{
    public void SetColor(string color)
    {
        string[] rgba = color.Split(", ");
        
        GetComponent<SpriteRenderer>().material.SetColor("_OutlineColor", new Color(float.Parse(rgba[0]), float.Parse(rgba[1]), float.Parse(rgba[2]), float.Parse(rgba[3])));
    }
    public void SetColor(Color color)
    {
        GetComponent<SpriteRenderer>().material.SetColor("_OutlineColor", color);
    }
}
