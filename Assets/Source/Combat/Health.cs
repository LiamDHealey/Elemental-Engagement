using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ElementalEngagement.Combat
{
    /// <summary>
    /// Component that allows something to take damage.
    /// </summary>
    public sealed class Health : MonoBehaviour
    {
        [Tooltip("The maximum health points this can have.")]
        [SerializeField] private float _maxHP = 100;
        public float maxHp
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        [Tooltip("Called when this is damaged.")]
        public UnityEvent<Damage> onDamaged;

        [Tooltip("Called once when this has been killed.")]
        public UnityEvent onKilled;


        // The current number of health points this has.
        public float hp
        {
            get { throw new System.NotImplementedException(); }
            private set { throw new System.NotImplementedException(); }
        }


        /// <summary>
        /// Cases this to take damage.
        /// </summary>
        /// <param name="damage"> The damage to take. </param>
        public void TakeDamage(Damage damage)
        {
            throw new System.NotImplementedException();
        }
    }
}