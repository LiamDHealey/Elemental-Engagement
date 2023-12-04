using ElementalEngagement.Combat;
using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
        public float requiredCapturePoints = 10;

        [Tooltip("The number of decapture points required to neutralize this location.")] [Min(0f)]
        public float requiredDecapturePoints = 10;

        [Tooltip("The multiplier applied to the favor given to a god to get the change in integrity. If a god is not present in this list than they cannot gain favor here.")]
        [SerializeField] private List<MinorGodToCapturePoints> capturePointSettings;

        [Tooltip("The hp lost per sacrifice.")]
        [SerializeField] private float sacrificeDamage = 4f;

        [Tooltip("The time between each sacrifice tick.")]
        [SerializeField] private float sacrificeInterval = 0.5f;

        [Tooltip("The amount time in seconds that this will be forced to remain neutral after being decapped.")]
        public float neutralLockoutTime = 4f;

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
        private Dictionary<SacrificeCommand, IEnumerator>  sacrificeCoroutines = new Dictionary<SacrificeCommand, IEnumerator>();

        //List of all units that have ever sacrificed and their cooldown
        private List<SacrificingUnit> unitsToUpdate = new List<SacrificingUnit>();

        // The amount of time remaining.
        public float remainingNeutralTime = 0f;

        private void Start()
        {
            if (allegiance.faction == Faction.Unaligned)
            {
                state = State.Neutral;
                onDecaptured?.Invoke();
            }
            else
            {
                state = State.Captured;
                onCaptured?.Invoke();
            }
        }

        private void Update()
        {
            if (IsGainingFavor())
            {
                FavorManager.ModifyFavor(allegiance.faction, allegiance.god, favorGainRate * Time.deltaTime);
            }
            remainingNeutralTime -= Time.deltaTime;

            for(int i = unitsToUpdate.Count - 1; i >= 0; i--)
            {
                if (unitsToUpdate[i].unit == null)
                {
                    unitsToUpdate.RemoveAt(i);
                }
            }

            SacrificeCurrentUnits();
        }

        private void SacrificeCurrentUnits()
        {
            MinorGod unitGod;
            Faction unitFaction;
            Health unitHealth;
            MinorGodToCapturePoints settings;

            foreach(SacrificingUnit targetUnit in unitsToUpdate)
            {
                if(!targetUnit.isActive)
                {
                    continue;
                }

                unitGod = targetUnit.unit.GetComponent<Allegiance>().god;
                unitFaction = targetUnit.unit.GetComponent<Allegiance>().faction;
                unitHealth = targetUnit.unit.GetComponent<Health>();
                settings = capturePointSettings.First(m => m.minorGod == unitGod);

                switch (state)
                {
                    case State.Neutral:
                        if (settings.allowCapture && remainingNeutralTime <= 0)
                        {
                            if (targetUnit.timeToWait <= 0)
                            {
                                state = State.Capturing;
                                foreach (var capturePoint in new Dictionary<Faction, float>(capturePoints))
                                {
                                    capturePoints[unitFaction] = 0f;
                                }
                                capturePoints[unitFaction] = settings.capturePointsPerSacrifice;
                                unitHealth.TakeDamage(new Damage(sacrificeDamage));
                                StartUnitSacrificing(targetUnit);
                            }
                        }
                        else
                        {
                            StopUnitSacrificing(targetUnit);
                        }
                        break;

                    case State.Capturing:
                        if (settings.allowCapture)
                        {
                            if (targetUnit.timeToWait <= 0)
                            {
                                capturePoints[unitFaction] += settings.capturePointsPerSacrifice;
                                unitHealth.TakeDamage(new Damage(sacrificeDamage));
                                if (capturePoints[unitFaction] >= requiredCapturePoints)
                                {
                                    state = State.Captured;
                                    allegiance.faction = unitFaction;
                                    onCaptured?.Invoke();
                                    StopUnitSacrificing(targetUnit);
                                }
                                else
                                {
                                    StartUnitSacrificing(targetUnit);
                                }
                            }
                        }
                        else
                        {
                            StopUnitSacrificing(targetUnit);
                        }
                        break;

                    case State.Decapturing:
                        if (unitFaction != allegiance.faction && settings.allowDecapture)
                        {
                            if (targetUnit.timeToWait <= 0)
                            {
                                decapturePoints += settings.decapturePointsPerSacrifice;
                                unitHealth.TakeDamage(new Damage(sacrificeDamage));
                                if (decapturePoints >= requiredDecapturePoints)
                                {
                                    state = State.Neutral;
                                    allegiance.faction = Faction.Unaligned;
                                    remainingNeutralTime = neutralLockoutTime;
                                    foreach (Faction faction in new List<Faction>(capturePoints.Keys))
                                    {
                                        capturePoints[faction] = 0;
                                    }
                                    onDecaptured?.Invoke();
                                    if (settings.allowCapture)
                                    {
                                        StartUnitSacrificing(targetUnit);
                                    }
                                    else
                                    {
                                        StopUnitSacrificing(targetUnit);
                                    }
                                }
                                else
                                {
                                    StartUnitSacrificing(targetUnit);
                                }
                            }
                        }
                        else
                        {
                            StopUnitSacrificing(targetUnit);
                        }
                        break;

                    case State.Captured:
                        if (settings.allowDecapture && allegiance.faction != unitFaction)
                        {
                            if(targetUnit.timeToWait <= 0)
                            {
                                decapturePoints = settings.decapturePointsPerSacrifice;
                                unitHealth.TakeDamage(new Damage(sacrificeDamage));
                                state = State.Decapturing;
                                StartUnitSacrificing(targetUnit);
                            }
                        }
                        else
                        {
                            StopUnitSacrificing(targetUnit);
                        }
                        break;
                }
                targetUnit.timeToWait -= Time.deltaTime;
            }
        }

        private void StartUnitSacrificing(SacrificingUnit targetUnit)
        {
            targetUnit.resetCooldown();
            if (!targetUnit.isActive)
            {
                targetUnit.isActive = true;
            }
        }

        private void StopUnitSacrificing(SacrificingUnit targetUnit)
        {
            if (targetUnit.isActive)
            {
                targetUnit.isActive = false;
            }
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
            for (int i = 0; i < unitsToUpdate.Count; i++)
            {
                if (unitsToUpdate[i].unit.Equals(unitToSacrifice))
                {
                    unitsToUpdate[i].isActive = true;
                    unitsToUpdate[i].unit.onSacrificeBegin?.Invoke();
                    return;
                }
            }

            unitToSacrifice.onSacrificeBegin?.Invoke();
            unitsToUpdate.Add(new SacrificingUnit(unitToSacrifice, sacrificeInterval));
        }


        /// <summary>
        /// Causes this to stop trying to sacrifice a unit.
        /// </summary>
        /// <param name="unitToSacrifice"> The unit being sacrificed. </param>
        public void StopSacrificing(SacrificeCommand unitToSacrifice)
        {
            for(int i = 0; i < unitsToUpdate.Count; i++) 
            {
                if (unitsToUpdate[i].unit.Equals(unitToSacrifice))
                {
                    unitsToUpdate[i].isActive = false;
                    unitsToUpdate[i].unit.onSacrificeEnd?.Invoke();
                }
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

        private class SacrificingUnit
        {
            public SacrificeCommand unit { get; set; }

            public float timeToWait { get; set; }

            private float cooldown;

            public bool isActive { get; set; }


            public SacrificingUnit(SacrificeCommand unit, float cooldown)
            {
                this.unit = unit;
                timeToWait = 0;
                this.cooldown = cooldown;
                isActive = true;
            }

            public void resetCooldown()
            {
                timeToWait = cooldown;
            }
        }
    }
}