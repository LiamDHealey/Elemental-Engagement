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
        [Tooltip("The amount to multiply the max hp by. When entering and leaving this area units stay at the same % of max health.")]
        [Min(0.00001f)]
        [SerializeField] private float maxHpMultiplier = 1;

        private void Start()
        {
            area.onTriggerEnter.AddListener(OnTriggerEntered);
            area.onTriggerExit.AddListener(OnTriggerExited);
        }
        private void OnDestroy()
        {
            foreach (Collider collider in area.overlappingColliders)
            {
                OnTriggerExited(collider);
            }
        }

        void OnTriggerEntered(Collider collider)
        {
            Health health = collider.GetComponent<Health>();

            if (health == null)
                return;
            if (!CanModify(collider))
                return;

            float hpPercent = health.hp / health.maxHp;
            health.maxHp *= maxHpMultiplier;
            health.hp = health.maxHp * hpPercent;
        }

        void OnTriggerExited(Collider collider)
        {
            Health health = collider.GetComponent<Health>();

            if (health == null)
                return;
            if (!CanModify(collider))
                return;

            float hpPercent = health.hp / health.maxHp;
            health.maxHp /= maxHpMultiplier;
            health.hp = health.maxHp * hpPercent;
        }
    }
}
