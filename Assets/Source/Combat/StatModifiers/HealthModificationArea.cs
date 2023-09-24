using System.Collections;
using System.Collections.Generic;
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


            void OnTriggerEnter(Collider collider)
            {
                Health health = collider.GetComponent<Health>();
                


                float hpPercent = health.hp / health.maxHp;

                health.maxHp += deltaMaxHp;

                health.hp = health.maxHp * hpPercent;

            }

            void OnTriggerExit(Collider collider)
            {
                Health health = collider.GetComponent<Health>();

                float hpPercent = health.hp / health.maxHp;

                health.maxHp -= deltaMaxHp;

                health.hp = health.maxHp * hpPercent;
            }

        }
    }
}