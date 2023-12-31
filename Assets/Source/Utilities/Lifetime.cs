using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ElementalEngagement.Utilities
{
    /// <summary>
    /// Destroys this game object after the lifetime has expired
    /// </summary>
    public class Lifetime : MonoBehaviour
    {
        [Tooltip("The amount of time this will exist for in seconds.")] [Min(0)]
        [SerializeField] public float lifetime = 1;

        [Tooltip("Game objects to detach before death")]
        [SerializeField] private List<GameObject> detachBeforeDeath = new List<GameObject>();

        public UnityEvent onDetached;

        private void Update()
        {
            lifetime -= Time.deltaTime;
            if (lifetime <= 0)
            {
                Detach();
            }
        }

        private void Detach()
        {
            for(int i = 0; i < detachBeforeDeath.Count; i++)
            {
                onDetached?.Invoke();
                detachBeforeDeath[i].transform.parent = null;
            }

            Destroy(gameObject);
        }
    }
}
