using ElementalEngagement.Favor;
using ElementalEngagement.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        private static Dictionary<(Faction, string), RaycastHit> _tagsToInteractibles = new Dictionary<(Faction, string), RaycastHit>();
        public static ReadOnlyDictionary<(Faction, string), RaycastHit> tagsToInteractibles => new(_tagsToInteractibles);

        [Tooltip("The mask to use when detecting locations where commands can be issued.")]
        [SerializeField] private LayerMask commandMask;


        [Tooltip("The tag of the units to rally to this location.")]
        [SerializeField] private string rallyTag;

        private void Start()
        {
            (Faction, string) key = (GetComponent<Allegiance>().faction, rallyTag);


            Ray findInteractibles = new Ray(new Vector3(transform.position.x, transform.position.y + 100, transform.position.z), -transform.up);
            RaycastHit colliderHit;
            Physics.Raycast(findInteractibles, out colliderHit, 9999f, commandMask);

            if (colliderHit.collider != null)
            {
                if(colliderHit.collider.GetComponent<SacrificeLocation>() != null)
                {
                    if (_tagsToInteractibles.ContainsKey(key))
                    {
                        _tagsToInteractibles[key] = colliderHit;
                    }
                    else
                    {
                        _tagsToInteractibles.Add(key, colliderHit);
                    }
                }
            }
            else
            {
                if (_tagsToInteractibles.ContainsKey(key))
                    _tagsToInteractibles.Remove(key);
            }

            NavMesh.SamplePosition(transform.position, out NavMeshHit worldHit, 50, NavMesh.AllAreas);
            transform.position = worldHit.position;

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
                _tagsToInteractibles.Remove(key);
            }
        }
    }
}