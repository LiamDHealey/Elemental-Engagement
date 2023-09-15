using ElementalEngagement.Favor;
using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
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
        { get; set; }

        [Tooltip("The amounts damage will be multiplied depending on the incoming damage's allegiance.")]
        [SerializeField] private List<DamageMultiplier> damageMultipliers;

        [Tooltip("Called when this is damaged.")]
        public UnityEvent<Damage> onDamaged;

        [Tooltip("Called once when this has been killed.")]
        public UnityEvent onKilled;


        // The current number of health points this has.
        public float hp
        { get; private set; }


        /// <summary>
        /// Cases this to take damage.
        /// </summary>
        /// <param name="damage"> The damage to take. </param>
        public void TakeDamage(Damage damage)
        {
            DamageMultiplier multiplier = new DamageMultiplier();
            Favor.MinorGod unitAffiliation = this.gameObject.GetComponent<Favor.MinorGod>();

            hp -= multiplier.checkAndMultiplyDamage(unitAffiliation, damage);
            onDamaged?.Invoke(damage);

            if(hp <= 0)
                onKilled?.Invoke();
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

            public float checkAndMultiplyDamage(MinorGod godOfUnit, Damage damage)
            {
                incomingAffiliation = damage.allegiance.god;
                if(incomingAffiliation.Equals(godOfUnit)) 
                {
                    return damage.amount;
                }
                else if(incomingAffiliation.Equals(Favor.MinorGod.Fire))
                { 
                    if(godOfUnit.Equals(Favor.MinorGod.Earth)) 
                    { 
                        return damage.amount * multiplier;
                    }
                }
                else if (incomingAffiliation.Equals(Favor.MinorGod.Water))
                {
                    if (godOfUnit.Equals(Favor.MinorGod.Fire))
                    {
                        return damage.amount * multiplier;
                    }
                }
                else if (incomingAffiliation.Equals(Favor.MinorGod.Earth))
                {
                    if (godOfUnit.Equals(Favor.MinorGod.Water))
                    {
                        return damage.amount * multiplier;
                    }
                }
                else 
                { 
                    return damage.amount; 
                }

                return 0;
            }
        }
    }
}