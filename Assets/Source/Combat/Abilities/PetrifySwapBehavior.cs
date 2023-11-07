using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PetrifySwapBehavior : MonoBehaviour
{

    [Tooltip("Material with the petrify texture that will be overlayed on the units.")]
    [SerializeField] Material petrifyMat;


    void Start()
    {
        petrifyMat = Resources.Load("Assets/Art/MapArt/Materials/EE_Textures_Petrify2.png", typeof(Material)) as Material;
    }

    public void applyPetrifyMaterial(GameObject[] units)
    {
        foreach (GameObject unit in units)
        {
            if (unit.GetType() == typeof(Unit))
            {
                unit.GetComponent<Renderer>().material = petrifyMat;
            }
        }
    }

    public void removePetrifyMaterial(GameObject[] units)
    {
        foreach (GameObject unit in units)
        {
            if (unit.GetType() == typeof(Unit))
            {
                unit.GetComponent<Renderer>().material = petrifyMat;
            }
        }
    }
}
