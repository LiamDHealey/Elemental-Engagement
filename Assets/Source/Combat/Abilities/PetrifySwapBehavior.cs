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

    //Queue of materials to re-apply to units once they leave the petrify radius.
    private Dictionary<Collider, Material> storedMats;

    private void Start()
    {
        storedMats = new Dictionary<Collider, Material>();
        area.onTriggerEnter.AddListener(OnTriggerEntered);
        area.onTriggerExit.AddListener(OnTriggerExited);
    }

    private void OnDestroy()
    {
        if (storedMats != null)
        {
            Debug.Log("StoredMats is not null");
            foreach (Collider key in storedMats.Keys)
            {
                var colliderUnit = key.gameObject.GetComponentInChildren<SpriteRenderer>();
                colliderUnit.material = storedMats[key];
                key.gameObject.GetComponentInChildren<Animator>().enabled = true;
            }
        }
    }

    private void OnTriggerEntered(Collider collider)
    {
        var colliderUnit = collider.gameObject.GetComponentInChildren<SpriteRenderer>();
        if (colliderUnit != null)
        {
            if (!CanModify(collider))
                return;

            storedMats.Add(collider, new Material(colliderUnit.material));
            Debug.Log("Added material!" + collider.gameObject.name);
            colliderUnit.material = petrifyMat;
            collider.gameObject.GetComponentInChildren<Animator>().enabled = false;
        }
    }


    private void OnTriggerExited(Collider collider)
    {
        var colliderUnit = collider.gameObject.GetComponentInChildren<SpriteRenderer>();
        if (colliderUnit != null && storedMats.ContainsKey(collider))
        {
            if (!CanModify(collider))
                return;

            collider.gameObject.GetComponentInChildren<SpriteRenderer>().material = storedMats[collider];
            collider.gameObject.GetComponentInChildren<Animator>().enabled = true;
        }
    }
}
