using ElementalEngagement.Combat;
using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace ElementalEngagement.Favor
{
    /// <summary>
    /// A location where units can be sacrificed.
    /// </summary>
    public class SacrificeLocation : MonoBehaviour
    {
        [Tooltip("What god and player to give the favor to.")]
        [SerializeField] private Allegiance allegiance;

        [Tooltip("The number of capture points required to claim this location.")] [Min(0f)]
        [SerializeField] public float requiredCapturePoints = 10;

        [Tooltip("The number of decapture points required to neutralize this location.")] [Min(0f)]
        [SerializeField] public float requiredDecapturePoints = 10;

        [Tooltip("The multiplier applied to the favor given to a god to get the change in integrity. If a god is not present in this list than they cannot gain favor here.")]
        [SerializeField] private List<MinorGodToCapturePoints> capturePointSettings;

        [Tooltip("The hp lost per sacrifice.")]
        [SerializeField] private float sacrificeDamage = 4f;

        [Tooltip("The time between each sacrifice tick.")]
        [SerializeField] private float sacrificeInterval = 0.5f;

        [Tooltip("The amount time in seconds that this will be forced to remain neutral after being decapped.")]
        [SerializeField] private float neutralLockoutTime = 4f;

        [Tooltip("The amount of favor gained every second while this is controlled.")]
        [SerializeField] private float favorGainRate = 4f;

        [Tooltip("Whether or not favor should be gained while being decaped.")]
        [SerializeField] private bool gainFavorWhileDecapping = true;


        [Tooltip("Called when this has been captured")]
        public UnityEvent onCaptured;

        [Tooltip("Called when this has been decaptured")]
        public UnityEvent onDecaptured;




        // The number of capture points this currently has.
        public State state { get; private set; }

        // The number of capture points this currently has.
        public Dictionary<Faction, float> capturePoints { get; private set; } = new() { { Faction.PlayerOne, 0 }, { Faction.PlayerTwo, 0 } };

        // The number of decapture points this currently has.
        public float decapturePoints { get; private set; } = 0;

        // Dictionary of coroutines currently being run associated with the unit running it.
        private Dictionary<SacrificeCommand, IEnumerator> sacrificeCoroutines = new Dictionary<SacrificeCommand, IEnumerator>();

        // The amount of time remaining.
        public float remainingNeutralTime = 0f;

        private void Start()
        {
            if (allegiance.faction == Faction.Unaligned)
            {
                state = State.Neutral;
            }
            else
            {
                state = State.Captured;
                onClaimed?.Invoke();
            }
        }

        private void Update()
        {
            if (IsGainingFavor())
            {
                FavorManager.ModifyFavor(allegiance.faction, allegiance.god, favorGainRate * Time.deltaTime);
            }
            remainingNeutralTime -= Time.deltaTime;
        }

        
        public bool IsGainingFavor()
        {
            return state == State.Captured || (gainFavorWhileDecapping && state == State.Decapturing);
        }

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
            bool unitSacrificing = false;
            MinorGod unitGod = targetUnit.GetComponent<Allegiance>().god;
            Faction unitFaction = targetUnit.GetComponent<Allegiance>().faction;
            Health unitUealth = targetUnit.GetComponent<Health>();
            MinorGodToCapturePoints settings = capturePointSettings.First(m => m.minorGod == unitGod);
            WaitForSeconds wait = new WaitForSeconds(sacrificeInterval);


            while (targetUnit)
            {
                switch (state)
                {
                    case State.Neutral:
                        if (settings.allowCapture && remainingNeutralTime <= 0)
                        {
                            state = State.Capturing;
                            foreach (var capturePoint in capturePoints)
                            {
                                capturePoints[unitFaction] = 0f;
                            }
                            capturePoints[unitFaction] = settings.capturePointsPerSacrifice;
                            unitUealth.TakeDamage(new Damage(sacrificeDamage));
                            StartUnitSacrificing();
                            yield return wait;
                        }
                        else
                        {
                            StopUnitSacrificing();
                            yield return null;
                        }
                        break;
                        


                    case State.Capturing:
                        if (settings.allowCapture)
                        {
                            capturePoints[unitFaction] += settings.capturePointsPerSacrifice;
                            unitUealth.TakeDamage(new Damage(sacrificeDamage));
                            if (capturePoints[unitFaction] >= requiredCapturePoints)
                            {
                                state = State.Captured;
                                allegiance.faction = unitFaction;
                                onCaptured?.Invoke();
                                StopUnitSacrificing();
                            }
                            else
                            {
                                StartUnitSacrificing();
                            }
                            yield return wait;
                        }
                        else
                        {
                            StopUnitSacrificing();
                            yield return null;
                        }
                        break;



                    case State.Decapturing:
                        if (unitFaction != allegiance.faction)
                        {
                            if (settings.allowDecapture)
                            {
                                decapturePoints += settings.decapturePointsPerSacrifice;
                                unitUealth.TakeDamage(new Damage(sacrificeDamage));
                                if (decapturePoints >= requiredDecapturePoints)
                                {
                                    state = State.Neutral;
                                    allegiance.faction = Faction.Unaligned;
                                    onDecaptured?.Invoke();
                                    if (settings.allowCapture)
                                    {
                                        StartUnitSacrificing();
                                    }
                                    else
                                    {
                                        StopUnitSacrificing();
                                    }
                                }
                                else
                                {
                                    StartUnitSacrificing();
                                }
                                yield return wait;
                            }
                            else
                            {
                                StopUnitSacrificing();
                                yield return null;
                            }
                        }
                        break;




                    case State.Captured:
                        if (settings.allowDecapture && allegiance.faction != unitFaction)
                        {
                            decapturePoints = settings.decapturePointsPerSacrifice;
                            unitUealth.TakeDamage(new Damage(sacrificeDamage));
                            state = State.Decapturing;
                            StartUnitSacrificing();
                            yield return wait;
                        }
                        else
                        {
                            StopUnitSacrificing();
                            yield return null;
                        }
                        break;
                }
            }




            void StartUnitSacrificing()
            {
                if (!unitSacrificing)
                {
                    unitSacrificing = true;
                    targetUnit.onSacrificeBegin?.Invoke();
                }
            }

            void StopUnitSacrificing()
            {
                if (unitSacrificing)
                {
                    unitSacrificing = false;
                    targetUnit.onSacrificeEnd?.Invoke();
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


        public enum State
        {
            Neutral,
            Capturing,
            Decapturing,
            Captured,
        }

        /// <summary>
        /// For storing how a god interacts with this location.
        /// </summary>
        [System.Serializable]
        private class MinorGodToCapturePoints
        {
            [Tooltip("The god to allowed to sacrifice here.")]
            public MinorGod minorGod;

            [Tooltip("The amount capture points gained when a unit associated with this god sacrifices once.")]
            public float capturePointsPerSacrifice = 1;

            [Tooltip("The amount capture points lost when a unit associated with this god sacrifices once.")]
            public float decapturePointsPerSacrifice = 1;

            [Tooltip("Whether units associated with this god can capture this location.")]
            public bool allowCapture = true;

            [Tooltip("Whether units associated with this god can decapture this location.")]
            public bool allowDecapture = true;
        }
    }
}