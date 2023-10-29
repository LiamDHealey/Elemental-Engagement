using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ElementalEngagement.Combat
{
    /// <summary>
    /// Used to buff/debuff health in an area.
    /// </summary>
    public class HealthModificationArea : StatModificationArea
    {
        [Tooltip("The amount to add to the max hp of any health in the area as a percent of its current value. When entering and leaving this area units stay at the same % of max health.")]
        [SerializeField] private float deltaMaxHp;

        private void Start()
        {
            area.onTriggerEnter.AddListener(OnTriggerEnter);
            area.onTriggerExit.AddListener(OnTriggerExit);
        }
        private void OnDestroy()
        {
            foreach (Collider collider in area.overlappingColliders)
            {
                Health health = collider.GetComponent<Health>();

                Allegiance all = collider.GetComponent<Allegiance>();

                // Check if health is null.
                if (health.Equals(null) || all == null)
                    continue;

                float hpPercent = health.hp / health.maxHp;

                health.maxHp -= deltaMaxHp;

                health.hp = health.maxHp * hpPercent;
            }
        }

        void OnTriggerEnter(Collider collider)
        {
            Health health = collider.GetComponent<Health>();

            Allegiance all = collider.GetComponent<Allegiance>();

            // Check if health is null.
            if (health.Equals(null) || all == null)
                return;

            //// If collider allegiance does not equal area of effect's allegiance.
            //if (all.faction == allegiance.faction)
            //    return;

            //// If collider god is not equal to the area of effect's god, or the area of effect's god is unaligned.
            //if (all.god != allegiance.god || allegiance.god == Favor.MinorGod.Unaligned)
            //    return;

            float hpPercent = health.hp / health.maxHp;

            health.maxHp *= deltaMaxHp;

            health.hp = health.maxHp * hpPercent;
        }

        void OnTriggerExit(Collider collider)
        {
            Allegiance all = collider.GetComponent<Allegiance>();

            Health health = collider.GetComponent<Health>();

            if (health == null || all == null)
                return;

            //// If collider allegiance does not equal area of effect's allegiance, or the area of effect's allegiance is unaligned.
            //if (all.faction != allegiance.faction || allegiance.faction == Faction.Unaligned)
            //    return;

            //// If collider god is not equal to the area of effect's god, or the area of effect's god is unaligned.
            //if (all.god != allegiance.god || allegiance.god == Favor.MinorGod.Unaligned)
            //    return;

            float hpPercent = health.hp / health.maxHp;

            health.maxHp /= deltaMaxHp;

            health.hp = health.maxHp * hpPercent;
        }
    }
}
