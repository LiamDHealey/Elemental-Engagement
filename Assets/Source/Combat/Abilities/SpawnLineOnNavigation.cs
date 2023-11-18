using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ElementalEngagement.Combat
{
    public class SpawnLineOnNavigation : MonoBehaviour, IAbilityCollider
    {
        [Tooltip("The game objects to pick from when duplicating on the navigation.")]
        [SerializeField] private List<GameObject> gameObjects = new List<GameObject>();

        [Tooltip("The distance between each duplicated element.")] [Min(0)]
        [SerializeField] private float distance = 30;

        [Tooltip("The maximum length of the duplication.")] [Min(0)]
        [SerializeField] private float maxLength = 500;

        [Tooltip("The minimum distance this must be from the nearest ability blocker.")]
        [SerializeField] private float blockerDistance = 20;

        [Tooltip("The mask used to detect what blocks this.")]
        [SerializeField] private LayerMask blockerMask;



        // All the object that were spawned by this
        private List<GameObject> spawnedObjects = new List<GameObject>();

        private bool _isColliding = false;
        bool IAbilityCollider.isColliding { get => _isColliding; set => _isColliding = value; }

        public void Spawn()
        {
            _isColliding = false;
            NavMesh.Raycast(transform.position, transform.position + transform.right * maxLength, out NavMeshHit endpoint1, NavMesh.AllAreas);
            NavMesh.Raycast(transform.position, transform.position + transform.right * -maxLength, out NavMeshHit endpoint2, NavMesh.AllAreas);

            float totalDistance = (endpoint1.position - endpoint2.position).magnitude;
            Vector3 direction = (endpoint2.position - endpoint1.position) / totalDistance;
            if (!float.IsFinite(totalDistance))
            {
                return;
            }

            if (Physics.SphereCast(new Ray(endpoint1.position, direction), blockerDistance, totalDistance, blockerMask))
            {
                _isColliding = true;
            }

            Vector3 start = endpoint1.position;
            Vector3 offset = direction * distance;
            for (int i = 0; i < totalDistance / distance; i++)
            {
                Spawn(start + offset * i);
            }
            Spawn(endpoint2.position);


            void Spawn(Vector3 position)
            {
                GameObject spawnedObject = Instantiate(gameObjects[Random.Range(0, gameObjects.Count)]);
                spawnedObject.transform.parent = transform;
                spawnedObject.transform.position = position;
                spawnedObject.transform.rotation = transform.rotation;
                spawnedObjects.Add(spawnedObject);
            }
        }

        public void Despawn()
        {
            foreach (GameObject spawnedObject in spawnedObjects)
            {
                Destroy(spawnedObject);
            }
        }
    }
}