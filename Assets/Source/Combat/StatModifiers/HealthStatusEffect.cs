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
    public class HealthStatusEffect : StatusEffect
    {
        [Tooltip("The amount to multiply the max hp by. When entering and leaving this area units stay at the same % of max health.")]
        [Min(0.00001f)]
        [SerializeField] private float maxHpMultiplier = 1;

        private Collider[] collidersToEffect;

        private void Start()
        {
            collidersToEffect = Physics.OverlapSphere(area.transform.position, area.radius);
            foreach (Collider collider in collidersToEffect)
            {
                Health health = collider.GetComponent<Health>();

                if (health == null)
                    continue;
                if (!CanModify(collider))
                    continue;

                float hpPercent = health.hp / health.maxHp;
                health.maxHp *= maxHpMultiplier;
                health.hp = health.maxHp * hpPercent;
            }
        }
        private void OnDestroy()
        {
            foreach (Collider collider in collidersToEffect)
            {
                Health health = collider.GetComponent<Health>();

                if (health == null)
                    continue;
                if (!CanModify(collider))
                    continue;

                float hpPercent = health.hp / health.maxHp;
                health.maxHp /= maxHpMultiplier;
                health.hp = health.maxHp * hpPercent;
            }
        }
    }
}
