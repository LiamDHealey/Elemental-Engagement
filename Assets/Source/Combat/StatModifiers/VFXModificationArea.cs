using ElementalEngagement.Player;
using ElementalEngagement.Utilities;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

namespace ElementalEngagement.Combat
{
    /// <summary>
    /// Used to buff/debuff navmesh agents in an area.
    /// </summary>
    public class VFXModificationArea : StatModificationArea
    {
        [Tooltip("The VFX to add to any units inside of the area.")]

        public GameObject VFXPrefab;

        public float destroyDelay;
        private Dictionary<Collider, GameObject> unitsToVFX = new Dictionary<Collider, GameObject>();

        void Start()
        {
            area.onTriggerEnter.AddListener(OnTriggerEntered);
            area.onTriggerExit.AddListener(OnTriggerExited);
        }

        public void OnDestroy()
        {
            foreach (Collider collider in area.overlappingColliders)
            {
                OnTriggerExited(collider);
            }
        }

        /// <summary>
        /// Activates when an object enters the object's collider area.
        /// If the object is a unit, then apply the Visual Effect to that unit.
        /// </summary>
        /// <param name="collider"></param>
        void OnTriggerEntered(Collider collider)
        {
            if (!CanModify(collider))
                return;


            if (unitsToVFX.ContainsKey(collider) && unitsToVFX[collider] != null && unitsToVFX[collider].GetComponent<Lifetime>() != null)
            {
                Destroy(unitsToVFX[collider].GetComponent<Lifetime>());
            }
            else
            {
                GameObject vfx = Instantiate(VFXPrefab, collider.transform);
                unitsToVFX.Remove(collider);
                unitsToVFX.Add(collider, vfx);
            }
        }

        /// <summary>
        /// Activates when an object exits the object's collider area.
        /// If the object is a unit, then remove the Visual Effect from that unit.
        /// </summary>
        /// <param name="collider"></param>
        void OnTriggerExited(Collider collider)
        {
            
            if (!CanModify(collider))
                return;

            unitsToVFX[collider].AddComponent<Lifetime>().lifetime = destroyDelay;
        }
    }
}
