using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElementalEngagement.Utilities
{
    /// <summary>
    /// Destroys this game object after the lifetime has expired
    /// </summary>
    public class Lifetime : MonoBehaviour
    {
        [Tooltip("The amount of time this will exist for in seconds.")] [Min(0)]
        [SerializeField] private float lifetime = 1;

        [Tooltip("Game objects to detach before death")]
        [SerializeField] private List<GameObject> detachBeforeDeath;

        void Start ()
        {
            Invoke("Detach", lifetime);
        }

        private void Detach()
        {
            for(int i = 0; i < detachBeforeDeath.Count; i++)
            {
                detachBeforeDeath[i].transform.parent = null;
            }

            Destroy(gameObject);
        }
    }
}
