using ElementalEngagement.Favor;
using ElementalEngagement.Player;
using ElementalEngagement.UI;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Unity.VisualScripting;
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
        { get { return _maxHP; }
          set
          {
                if(value <= 0)
                {
                    return;
                }
                else
                {
                    _maxHP = value;
                }

                if(_maxHP < hp)
                {
                    hp = _maxHP;
                }
          }
        }

        [Tooltip("The amounts damage will be multiplied depending on the incoming damage's allegiance.")]
        [SerializeField] private List<DamageMultiplier> damageMultipliers;

        [Tooltip("How long the health bar shows up when damage is taken")]
        [SerializeField] public float damageHealthBarTime;

        [Tooltip("Called when this is damaged.")]
        public UnityEvent<Damage> onDamaged;

        [Tooltip("Called once when this has been killed.")]
        public UnityEvent onKilled;

        // The current number of health points this has.
        public float hp
        { get;  set; }

        public void Awake()
        {
            hp = _maxHP;
        }

        /// <summary>
        /// Causes this to take damage.
        /// </summary>
        /// <param name="damage"> The damage to take. </param>
        public void TakeDamage(Damage damage)
        {
            if (hp <= 0)
                return;

            for(int i = 0; i < damageMultipliers.Count; i++)
            {
                if (damageMultipliers[i].incomingAffiliation.Equals(damage.allegiance?.god))
                {
                    damage.amount *= damageMultipliers[i].multiplier;
                }
            }

            onDamaged?.Invoke(damage);

            hp = Mathf.Clamp(hp - damage.amount, 0, maxHp);

            if(hp <= 0)
                onKilled?.Invoke();

            StartCoroutine(showHealthBar());
        }

        /// <summary>
        /// Show the health bar for a set amount of seconds
        /// </summary>
        IEnumerator showHealthBar()
        {
            gameObject.GetComponentInChildren<HealthBar>().FadeIn();
            yield return new WaitForSeconds(damageHealthBarTime);
            gameObject.GetComponentInChildren<HealthBar>().FadeOut();
        }

        /// <summary>
        /// Used for determine how things resist damage.
        /// </summary>
        [System.Serializable]
        private class DamageMultiplier
        {
            [Tooltip("The god associated with the incoming damage.")]
            public Favor.MinorGod incomingAffiliation;

            [Tooltip("The amount that damage willed be multiplied by.")]
            public float multiplier = 1.5F;
        }
    }
}