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

        /// <summary>
        /// Checks all objects in the area of effect and applies proper changes to each one
        /// </summary>
        private void Start()
        {
            if (singleTarget)
            {
                if (gameObject.transform.parent == null) { return; }

                Health health = gameObject.transform.parent.GetComponent<Health>();

                if (health == null)
                    return;
                if (!CanModify(gameObject.transform.parent.GetComponent<Collider>()))
                    return;

                float hpPercent = health.hp / health.maxHp;
                health.maxHp *= maxHpMultiplier;
                health.hp = health.maxHp * hpPercent;
                return;
            }

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

        /// <summary>
        /// Same as start, but restores values to original
        /// </summary>
        private void OnDestroy()
        {
            if (singleTarget)
            {
                if (gameObject.transform.parent == null) { return; }

                Health health = gameObject.transform.parent.GetComponent<Health>();

                if (health == null)
                    return;
                if (!CanModify(gameObject.transform.parent.GetComponent<Collider>()))
                    return;

                float hpPercent = health.hp / health.maxHp;
                health.maxHp /= maxHpMultiplier;
                health.hp = health.maxHp * hpPercent;
                return;
            }

            foreach (Collider collider in collidersToEffect)
            {
                if (collider == null)
                    continue;

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
