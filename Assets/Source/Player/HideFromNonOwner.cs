using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class HideFromNonOwner : MonoBehaviour
{
    public bool invertAllegiance = false;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer(
            invertAllegiance != (GetComponentInParent<Allegiance>().faction != Faction.PlayerOne) 
            ? "HiddenFromP1" 
            : "HiddenFromP2");
    }
}
