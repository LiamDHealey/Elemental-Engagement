using ElementalEngagement.Favor;
using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SearchService;

namespace ElementalEngagement.Combat
{
    public class UnitUpgrader : MonoBehaviour
    {
        [Tooltip("The prefab to replaced the upgraded units with.")]
        [SerializeField] private GameObject upgradedPrefab;

        [Tooltip("The radius to upgrade units in.")]
        [SerializeField] private float radius = 2f;

        [Tooltip("The layer that all units that will be replaced must be in.")]
        [SerializeField] private LayerMask mask;

        [Tooltip("The tag that must be on all units that will be replaced.")]
        [SerializeField] private string unitTag;

        [Tooltip("The allegiance that all units that will be replaced must be aligned with.")]
        [SerializeField] private Allegiance allegiance;


        private void Start()
        {
            foreach(Collider collider in Physics.OverlapSphere(transform.position, radius, mask))
            {
                if (!collider.CompareTag(unitTag))
                    continue;

                Allegiance colliderAllegiance = collider.GetComponent<Allegiance>();
                if (!allegiance.CheckBothAllegiance(colliderAllegiance))
                    continue;



                GameObject upgradedUnit = Instantiate(upgradedPrefab);

                upgradedUnit.transform.position = collider.transform.position;

                Health colliderHealth = collider.GetComponent<Health>();
                Health upgradedHealth = upgradedUnit.GetComponent<Health>();
                float healthPercent = colliderHealth.hp / colliderHealth.maxHp;
                upgradedHealth.hp = upgradedHealth.maxHp * healthPercent;

                upgradedUnit.GetComponent<Allegiance>().faction = colliderAllegiance.faction;

                Destroy(collider.gameObject);

                Spawner.spawnedObjects[colliderAllegiance.faction].Add(upgradedUnit);
                upgradedUnit.GetComponent<Health>().onKilled.AddListener(() => Spawner.spawnedObjects[colliderAllegiance.faction].Remove(upgradedUnit));
            }
        }
    }

}
