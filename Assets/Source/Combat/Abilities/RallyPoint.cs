using ElementalEngagement.Favor;
using ElementalEngagement.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace ElementalEngagement.Combat
{
    [RequireComponent(typeof(Allegiance))]
    public class RallyPoint : MonoBehaviour
    {
        private static Dictionary<(Faction, string), Transform> _tagsToRallyLocations = new Dictionary<(Faction, string), Transform>();
        public static ReadOnlyDictionary<(Faction, string), Transform> tagsToRallyLocations => new(_tagsToRallyLocations);


        [Tooltip("The tag of the units to rally to this location.")]
        [SerializeField] private string rallyTag;

        private void Start()
        {
            (Faction, string) key = (GetComponent<Allegiance>().faction, rallyTag);

            NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 50, NavMesh.AllAreas);
            transform.position = hit.position;

            if (_tagsToRallyLocations.ContainsKey(key))
            {
                Destroy(_tagsToRallyLocations[key].gameObject);
                _tagsToRallyLocations[key] = transform;
            }
            else
            {
                _tagsToRallyLocations.Add(key, transform);
            }
        }

        private void OnDestroy()
        {
            (Faction, string) key = (GetComponent<Allegiance>().faction, rallyTag);

            if (_tagsToRallyLocations.TryGetValue(key, out Transform value) && value == transform)
            {
                _tagsToRallyLocations.Remove(key);
            }
        }
    }
}