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

        /// <summary>
        /// Checks all objects in the area of effect and applies proper changes to each one
        /// </summary>
        void Start()
        {
            if (gameObject.transform.parent == null)
                return;

            if (singleTarget)
            {
                Movement speed= gameObject.transform.parent.GetComponent<Movement>();

                if (speed == null)
                    return;
                if (!CanModify(gameObject.transform.parent.GetComponent<Collider>()))
                    return;

                speed.speed *= speedMultiplier;
                return;
            }

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

        /// <summary>
        /// Same as start, but restores values to original
        /// </summary>
        public void OnDestroy()
        {
<<<<<<< Updated upstream
            if (!enabled)
            {
                return;
            }
=======
            if (gameObject.transform.parent == null)
                return;
>>>>>>> Stashed changes

            if (singleTarget)
            {
                Movement speed = gameObject.transform.parent.GetComponent<Movement>();

                if (speed == null)
                    return;
                if (!CanModify(gameObject.transform.parent.GetComponent<Collider>()))
                    return;

                speed.speed /= speedMultiplier;
                return;
            }

            foreach (Collider collider in collidersToEffect)
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
