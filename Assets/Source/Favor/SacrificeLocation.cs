using ElementalEngagement.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElementalEngagement.Favor
{
    /// <summary>
    /// A location where units can be sacrificed.
    /// </summary>
    public class SacrificeTarget : MonoBehaviour
    {
        [field: Tooltip("The maximum integrity of this. This is unable to sacrifice any unit that would move integrity outside its acceptable range.")]
        [field: SerializeField] public float maxIntegrity { get; private set; } = 0.5f;

        [field: Tooltip("The maximum integrity of this. This is unable to sacrifice any unit that would move integrity outside its acceptable range.")]
        [field: SerializeField] public float integrity { get; private set; } = 0.25f;

        [Tooltip("The multiplier applied to the favor given to a god to get the change in integrity. If a god is not present in this list than they cannot gain favor here.")]
        [SerializeField] private List<MinorGodToIntegrityMultiplier> minorGodsToIntegrityMultipliers;

        [Tooltip("The hp lost per sacrifice.")]
        [SerializeField] private float sacrificeDamage = 4f;

        [Tooltip("The time between each sacrifice tick.")]
        [SerializeField] private float sacrificeInterval = 0.5f;


        /// <summary>
        /// Tries to sacrifice a unit. Will succeed if the integrity gained/lost will not put it outside the acceptable range.
        /// If it succeeds kill the unit and give its favor gain to the associated player and god.
        /// </summary>
        /// <param name="unitToSacrifice"> The unit being sacrificed. </param>
        /// <returns> True if the unit was sacrificed. </returns>
        public bool Sacrifice(SacrificeCommand unitToSacrifice)
        {
            throw new System.NotImplementedException();
        }


        /// <summary>
        /// For storing how a god interacts with this location.
        /// </summary>
        [System.Serializable]
        private class MinorGodToIntegrityMultiplier
        {
            [Tooltip("The god to allow to sacrifice here.")]
            public MinorGod minorGod;

            [Tooltip("The amount favor gains are multiplied by to get the change in integrity.")]
            public float integrityMultiplier = 1;

            [Tooltip("The amount favor gains are multiplied for this god.")]
            public float favorMultiplier = 1;
        }
    }
}