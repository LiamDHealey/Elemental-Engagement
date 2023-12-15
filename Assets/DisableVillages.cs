using ElementalEngagement.Combat;
using ElementalEngagement.Favor;
using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DisableVillages : MonoBehaviour
{
    List<Transform> villages;
    // Start is called before the first frame update
    void Start()
    {
        villages = FindObjectsOfType<SacrificeLocation>()
        .Where(village => village.GetComponent<Allegiance>().god == MinorGod.Human)
        .Select(spawner => spawner.transform)
        .ToList();

        foreach (Transform village in villages)
        {
            Debug.Log(village);
        }
    }

    public void StopTrackingVillages()
    {
        foreach(Transform village in villages)
        {
            Debug.Log(village);
            village.GetComponent<SacrificeLocation>().enabled = false;
        }
    }
}
