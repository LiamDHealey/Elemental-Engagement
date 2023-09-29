using ElementalEngagement.Combat;
using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElementalEngagement.Favor
{
    /// <summary>
    /// A location where units can be sacrificed.
    /// </summary>
    public class SacrificeLocation : MonoBehaviour
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

        [Tooltip("Whether this location is being sacrificed to or not.")]
        [SerializeField] private bool isSacrificed;

        [Tooltip("Unit currently being sacrificed. If there is none, this is null")]
        [SerializeField] private SacrificeCommand targetUnit;


        /// <summary>
        /// Repeatedly tries to sacrifice a unit. Will succeed if the integrity gained/lost will not put it outside the acceptable range.
        /// If it succeeds it will damage the unit and give its favor gain to the associated player and god. If it fails it will cancel the sacrifice command.
        /// </summary>
        /// <param name="unitToSacrifice"> The unit being sacrificed. </param>
        public void StartSacrificing(SacrificeCommand unitToSacrifice)
        {
            isSacrificed = true;
            targetUnit = unitToSacrifice;
        }

        private void Update()
        {
            if (isSacrificed == true && integrity < maxIntegrity && targetUnit)
            {
                MinorGod unitGod = targetUnit.GetComponent<Allegiance>().god;
                float addToIntegrity = 0;
                foreach (MinorGodToIntegrityMultiplier multiplier in minorGodsToIntegrityMultipliers)
                {
                    if (multiplier.minorGod == unitGod)
                    {
                        addToIntegrity = multiplier.integrityMultiplier;
                        FavorManager.ModifyFavor(targetUnit.GetComponent<Faction>(), unitGod, multiplier.favorMultiplier);
                        //TODO: Reduce player's health when sacrifice succeeds
                    }
                }
                integrity += addToIntegrity;
            } else
            {
                targetUnit = null;
            }
        }

        /// <summary>
        /// Causes this to stop trying to sacrifice a unit.
        /// </summary>
        /// <param name="unitToSacrifice"> The unit being sacrificed. </param>
        public void StopSacrificing(SacrificeCommand unitToSacrifice)
        {
            isSacrificed = false;
            targetUnit = null;
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