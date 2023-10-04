using ElementalEngagement.Combat;
using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ElementalEngagement.Favor
{
    /// <summary>
    /// A location where units can be sacrificed.
    /// </summary>
    public class SacrificeLocation : MonoBehaviour
    {
        [Tooltip("If not unaligned fill I favor for this god instead.")]
        [SerializeField] private MinorGod godOverride;

        [field: Tooltip("The maximum integrity of this. This is unable to sacrifice any unit that would move integrity outside its acceptable range.")]
        [field: SerializeField] public float maxIntegrity { get; private set; } = 0.5f;

        [field: Tooltip("The maximum integrity of this. This is unable to sacrifice any unit that would move integrity outside its acceptable range.")]
        [field: SerializeField] public float integrity { get; private set; } = 0.25f;

        [Tooltip("The multiplier applied to the favor given to a god to get the change in integrity. If a god is not present in this list than they cannot gain favor here.")]
        [SerializeField] private List<MinorGodToIntegrity> minorGodsToIntegrityMultipliers;

        [Tooltip("The hp lost per sacrifice.")]
        [SerializeField] private float sacrificeDamage = 4f;

        [Tooltip("The time between each sacrifice tick.")]
        [SerializeField] private float sacrificeInterval = 0.5f;

        [Tooltip("Dictionary of coroutines currently being run associated with the unit running it.")]
        private Dictionary<SacrificeCommand, IEnumerator> sacrificeCoroutines = new Dictionary<SacrificeCommand, IEnumerator>();


        /// <summary>
        /// Repeatedly tries to sacrifice a unit. Will succeed if the integrity gained/lost will not put it outside the acceptable range.
        /// If it succeeds it will damage the unit and give its favor gain to the associated player and god. If it fails it will cancel the sacrifice command.
        /// </summary>
        /// <param name="unitToSacrifice"> The unit being sacrificed. </param>
        public void StartSacrificing(SacrificeCommand unitToSacrifice)
        {
            IEnumerator sacrificeCoroutine = SacrificeUnits(unitToSacrifice);
            sacrificeCoroutines.Add(unitToSacrifice, sacrificeCoroutine);
            StartCoroutine(sacrificeCoroutines[unitToSacrifice]);
        }

        /// <summary>
        /// Coroutine for sacrificing units. Runs infinitely at every sacrificeInterval until stopped
        /// by another method.
        /// </summary>
        /// <param name="targetUnit">The unit that will be calling on this coroutine.</param>
        /// <param name="isSacrificing">If the coroutine is being started, set to true. If being stopped, set to false</param>
        /// <returns></returns>
        private IEnumerator SacrificeUnits(SacrificeCommand targetUnit)
        {
            bool unitSacrificing = true;
            MinorGod unitGod = targetUnit.GetComponent<Allegiance>().god;
            MinorGodToIntegrity multiplier = minorGodsToIntegrityMultipliers.First(m => m.minorGod == unitGod);

            while (targetUnit)
            {
                float newIntegrity = integrity + multiplier.deltaIntegrity;
                if (newIntegrity >= 0 && newIntegrity <= maxIntegrity)
                {
                    integrity = newIntegrity;
                    FavorManager.ModifyFavor(targetUnit.GetComponent<Allegiance>().faction, godOverride == MinorGod.Unaligned ? unitGod : godOverride, multiplier.deltaFavor);
                    targetUnit.GetComponent<Health>().TakeDamage(new Damage(sacrificeDamage));

                    if (!unitSacrificing)
                    {
                        unitSacrificing = true;
                        targetUnit.onSacrificeBegin?.Invoke();
                    }

                    yield return new WaitForSeconds(sacrificeInterval);
                }
                else
                {

                    if (unitSacrificing)
                    {
                        unitSacrificing = false;
                        targetUnit.onSacrificeEnd?.Invoke();
                    }
                    yield return null;
                }
            }
        }

        /// <summary>
        /// Causes this to stop trying to sacrifice a unit.
        /// </summary>
        /// <param name="unitToSacrifice"> The unit being sacrificed. </param>
        public void StopSacrificing(SacrificeCommand unitToSacrifice)
        {
            if (sacrificeCoroutines.ContainsKey(unitToSacrifice))
            {
                IEnumerator coroutineToStop = sacrificeCoroutines[unitToSacrifice];
                StopCoroutine(coroutineToStop);
                sacrificeCoroutines.Remove(unitToSacrifice);
            }
        }


        /// <summary>
        /// For storing how a god interacts with this location.
        /// </summary>
        [System.Serializable]
        private class MinorGodToIntegrity
        {
            [Tooltip("The god to allow to sacrifice here.")]
            public MinorGod minorGod;

            [Tooltip("The amount favor gains are multiplied by to get the change in integrity.")]
            public float deltaIntegrity = 1;

            [Tooltip("The amount favor gains are multiplied for this god.")]
            public float deltaFavor = 1;
        }
    }
}