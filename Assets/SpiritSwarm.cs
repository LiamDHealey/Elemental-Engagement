using ElementalEngagement;
using ElementalEngagement.Combat;
using ElementalEngagement.Favor;
using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpiritSwarm : MonoBehaviour
{

    [Tooltip("The Collider used to detect collisions")]
    [SerializeField] private BindableCollider area;

    [Tooltip("The GameObject Status Effect to apply")]
    [SerializeField] private GameObject template;

    [Tooltip("This power's Allegiance")]
    [SerializeField] private Allegiance allegiance;

    private void Start()
    {
        area.onTriggerEnter.AddListener(RemoveStatusEffect);
        area.onTriggerExit.AddListener(AssignStatusEffect);
    }

    public void RemoveStatusEffect(Collider collider)
    {
        if(collider.transform.GetComponentsInChildren<AttackStatusEffect>() != null)
        {
            AttackStatusEffect[] effects = collider.transform.GetComponentsInChildren<AttackStatusEffect>();

            foreach(AttackStatusEffect effect in effects) 
            {
                if (effect.gameObject.name == "SpiritStatusEffect(Clone)")
                {
                    Destroy(effect.gameObject);
                }
            }
        }
    }

    public void AssignStatusEffect(Collider collider)
    {
        Allegiance colliderAllegiance = collider.GetComponent<Allegiance>();
        if (colliderAllegiance == null)
            return;
        if (((allegiance.faction == colliderAllegiance?.faction) || allegiance.faction == Faction.Unaligned) 
            && (allegiance.god == colliderAllegiance?.god || allegiance.god == MinorGod.Unaligned))
        {
            GameObject clone = Instantiate(template.gameObject, collider.gameObject.transform);
            clone.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        foreach(Collider unit in area.overlappingColliders)
        {
            if (unit != null)
                AssignStatusEffect(unit);
        }
    }
}
