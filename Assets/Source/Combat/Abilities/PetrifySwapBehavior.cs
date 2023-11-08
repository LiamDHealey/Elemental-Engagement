using ElementalEngagement.Combat;
using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.CanvasScaler;

public class PetrifySwapBehavior : StatModificationArea
{

    [Tooltip("Material with the petrify texture that will be overlayed on the units.")]
    [SerializeField] Material petrifyMat;

    [Tooltip("Material with the team glow that is stored when the trigger enters and re-applied once the trigger exits. DO NOT TOUCH THIS UNLESS YOU'RE EDITING THE ACTUAL SCRIPT!")]
    [SerializeField] Material storedMat;

    private void Start()
    {
        area.onTriggerEnter.AddListener(OnTriggerEntered);
        area.onTriggerExit.AddListener(OnTriggerExited);
    }

    private void OnTriggerEntered(Collider collider)
    {
        Debug.Log(collider.gameObject.name + " entered");
        storedMat = collider.gameObject.GetComponent<Renderer>().material;
        collider.gameObject.GetComponent<Renderer>().material = petrifyMat;
    }


    private void OnTriggerExited(Collider collider)
    {
        Debug.Log(collider.gameObject.name + " exited");
        collider.gameObject.GetComponent<Renderer>().material = storedMat;
    }
}
