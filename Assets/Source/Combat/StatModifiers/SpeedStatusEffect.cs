using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.VisualScripting;
using UnityEngine;

namespace ElementalEngagement.Combat
{
    public class SpeedStatusEffect : StatusEffect
    {
        [Tooltip("The amount to multiply the speed of any units in this area by.")]
        [Min(0.00001f)]
        [SerializeField] private float speedMultiplier = 1;

        private Collider[] collidersToEffect;

        void Start()
        {
            collidersToEffect = Physics.OverlapSphere(area.transform.position, area.radius);
            foreach (Collider collider in collidersToEffect)
            {
                Movement speed = collider.GetComponent<Movement>();

                if (speed == null)
                    continue;
                if (!CanModify(collider))
                    continue;

                speed.speed *= speedMultiplier;
            }
        }

        public void OnDestroy()
        {
            foreach(Collider collider in collidersToEffect)
            {
                Movement speed = collider.GetComponent<Movement>();

                if (speed == null)
                    continue;
                if (!CanModify(collider))
                    continue;

                speed.speed /= speedMultiplier;
            }
        }
    }
}
