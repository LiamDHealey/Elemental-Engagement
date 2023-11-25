using ElementalEngagement.Combat;
using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.CanvasScaler;

public class PetrifySwapBehavior : StatusEffect
{

    [Tooltip("Material with the petrify texture that will be overlayed on the units.")]
    [SerializeField] Material petrifyMat;

    //Queue of materials to re-apply to units once they leave the petrify radius.
    private Dictionary<Collider, Material> storedMats;

    private Collider[] collidersToEffect;

    private void Start()
    {
        storedMats = new Dictionary<Collider, Material>();
        collidersToEffect = Physics.OverlapSphere(area.transform.position, area.radius);
        foreach (Collider collider in collidersToEffect)
        {
            var colliderUnit = collider.gameObject.GetComponentInChildren<SpriteRenderer>();
            if (colliderUnit != null)
            {
                if (!CanModify(collider))
                    continue;
                if (collider.GetComponent<Movement>() == null)
                    continue;

                storedMats.Add(collider, new Material(colliderUnit.material));
                Debug.Log("Added material!" + collider.gameObject.name);
                colliderUnit.material = petrifyMat;
                collider.gameObject.GetComponentInChildren<Animator>().enabled = false;
            }
        }
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
}
