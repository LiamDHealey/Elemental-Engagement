using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElementalEngagement.Combat
{
    /// <summary>
    /// Used to buff/debuff attacks in an area.
    /// </summary>
    public class AttackModificationArea : StatModificationArea
    {
        [Tooltip("The amount to add to the damage of any attacks in the area as a percent of its current value.")]
        [SerializeField] private float damageMultiplier;

        [Tooltip("The amount to add to the knockback amount of any attacks in the area as a percent of its current value.")]
        [SerializeField] private float knockbackMultiplier;

        [Tooltip("The amount to add to the attack interval of any attacks in the area as a percent of its current value.")]
        [SerializeField] private float attackIntervalMutliplier;


        private void Start()
        {
            area.onTriggerEnter.AddListener(OnTriggerEnter);
            area.onTriggerExit.AddListener(OnTriggerExit);
        }

        public void OnDestroy()
        {
            foreach(Collider collider in area.overlappingColliders)
            {
                Allegiance all = collider.GetComponent<Allegiance>();

                Attack attack = collider.GetComponent<Attack>();

                if (attack == null || all == null)
                    return;

                attack.damage.amount /= damageMultiplier;

                attack.knockback.amount /= knockbackMultiplier;

                attack.attackInterval /= attackIntervalMutliplier;
            }
        }

        /// <summary>
        /// Activates when an object enters the object's collider area.
        /// If the object is a unit, then apply the damage, knockback, and attack interval upgrades
        /// to that unit.
        /// </summary>
        /// <param name="collider"></param>
        void OnTriggerEnter(Collider collider)
        {
            Allegiance all = collider.GetComponent<Allegiance>();

            Attack attack = collider.GetComponent<Attack>();

            if (attack == null || all == null)
                return;

            //// If collider god is not equal to the area of effect's god, or the area of effect's god is unaligned.
            //if (all.faction != allegiance.faction || allegiance.faction == Faction.Unaligned)
            //    return;

            //if (all.god != allegiance.god || allegiance.god == Favor.MinorGod.Unaligned)
            //    return;

            attack.damage.amount *= damageMultiplier;

            attack.knockback.amount *= knockbackMultiplier;

            attack.attackInterval *= attackIntervalMutliplier;
        }

        /// <summary>
        /// Activates when an object exits the object's collider area.
        /// If the object is a unit, then apply the damage, knockback, and attack interval downgrades
        /// to that unit.
        /// </summary>
        /// <param name="collider"></param>
        void OnTriggerExit(Collider collider)
        {
            Allegiance all = collider.GetComponent<Allegiance>();

            Attack attack = collider.GetComponent<Attack>();

            if (attack == null || all == null)
                return;

            //// If collider allegiance does not equal area of effect's allegiance, or the area of effect's allegiance is unaligned.
            //if (all.faction != allegiance.faction || allegiance.faction == Faction.Unaligned)
            //    return;

            //// If collider god is not equal to the area of effect's god, or the area of effect's god is unaligned.
            //if (all.god != allegiance.god || allegiance.god == Favor.MinorGod.Unaligned)
            //    return;

            attack.damage.amount /= damageMultiplier;

            attack.knockback.amount /= knockbackMultiplier;

            attack.attackInterval /= attackIntervalMutliplier;
        }
    }
}
