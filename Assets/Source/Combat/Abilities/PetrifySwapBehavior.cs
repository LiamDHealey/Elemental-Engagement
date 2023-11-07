using ElementalEngagement.Combat;
using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PetrifySwapBehavior : StatModificationArea
{

    [Tooltip("Material with the petrify texture that will be overlayed on the units.")]
    [SerializeField] Material petrifyMat;


    private void Start()
    {
        area.onTriggerEnter.AddListener(OnTriggerEntered);
        area.onTriggerExit.AddListener(OnTriggerExited);
    }

    private void OnTriggerEntered(Collider collider)
    {
        applyPetrifyMaterial(collider.gameObject);
    }


    private void OnTriggerExited(Collider collider)
    {
        removePetrifyMaterial(collider.gameObject);
    }

    //Internal helper method to apply the petrify material to the unit
    private void applyPetrifyMaterial(GameObject unit)
    {
        unit.GetComponent<Renderer>().material = petrifyMat;
    }

    //Internal helper method to remove the petrify material to the unit
    private void removePetrifyMaterial(GameObject unit)
    {
        Destroy(unit.GetComponent<Renderer>().material);
    }
}
