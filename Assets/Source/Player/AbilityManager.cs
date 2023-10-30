using ElementalEngagement.Combat;
using ElementalEngagement.Favor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static ElementalEngagement.Favor.FavorProgressionSettings.GodProgressionSettings;
using static UnityEngine.InputSystem.InputAction;

namespace ElementalEngagement.Player
{
    /// <summary>
    /// Responsible for handling ability related input, spawning ability effect, setting their aliment (if applicable), and handling ability cooldowns.
    /// </summary>
    [RequireComponent(typeof(Allegiance))]
    public class AbilityManager : MonoBehaviour
    {
        [Tooltip("Called when the selected ability changes.")]
        public UnityEvent<Ability> onSelectedAbilityChanged;

        [Tooltip("Called when an ability was played.")]
        public UnityEvent<Ability> onAbilityPlayed;

        [Tooltip("Called when an ability is unlocked.")]
        public UnityEvent<Ability> onAbilityUnlocked;

        [Tooltip("Called when an ability is locked.")]
        public UnityEvent<Ability> onAbilityLocked;

        [Tooltip("The cursor used to get the ability play location from.")]
        [SerializeField] private PlayerCursor cursor;

        [Tooltip("The input to get the play bindings from.")]
        [SerializeField] private PlayerInput input;

        [Tooltip("The mask to use when detecting where to spawn an ability.")]
        [SerializeField] private LayerMask abilityMask;

        // The currently selected ability
        public Ability selectedAbility { get; private set; }


        // The current abilities' remaining cooldown times in seconds.
        private Dictionary<Ability, float> _abilityCooldowns = new Dictionary<Ability, float>();
        public ReadOnlyDictionary<Ability, float> abilityCooldowns { get => new ReadOnlyDictionary<Ability, float>(_abilityCooldowns); }

        // The allegaince of this.
        public Allegiance allegiance { get; private set; }

        // The keybindings of the actions.
        private Dictionary<Ability, Action<CallbackContext>> actionBindings;

        // All the abilities available in this game.
        private HashSet<AbilityUnlock> abilityUnlocks;

        // The abilities that have been unlocked.
        private HashSet<Ability> unlockedAbilities;

        // The abilities that have been locked.
        private HashSet<Ability> lockedAbilities = new HashSet<Ability>();

        // The number of unlocked abilities in each tier.
        private List<int> abilitiesInTiers = new List<int>();

        private void Awake() => allegiance = GetComponent<Allegiance>();

        private void Start()
        {
            FavorManager.onFavorChanged.AddListener(OnFavorChanged);

            abilityUnlocks = FavorManager.godProgressionSettings.Values
                .SelectMany(setting => setting.abilityUnlocks)
                .ToHashSet();

            unlockedAbilities = FavorManager.godProgressionSettings.Values
                .SelectMany(setting => setting.abilityUnlocks)
                .Where(unlock => unlock.favorThreshold <= 0)
                .Select(unlock => unlock.ability)
                .ToHashSet();
            foreach (Ability unlockedAbility in unlockedAbilities)
            {
                onAbilityUnlocked?.Invoke(unlockedAbility);
            }


            actionBindings = new Dictionary<Ability, Action<CallbackContext>>();
            foreach (AbilityUnlock abilityUnlock in abilityUnlocks)
            {
                Ability ability = abilityUnlock.ability;
                void OnAbilityPlayed(CallbackContext context)
                {
                    if (!unlockedAbilities.Contains(ability))
                        return;

                    _abilityCooldowns.Add(ability, ability.cooldown);
                    if (cursor.RayUnderCursor(out Ray ray) && Physics.Raycast(ray, out RaycastHit hit, 9999f, abilityMask))
                    {
                        GameObject abilityObject = Instantiate(ability.prefabToSpawn);
                        abilityObject.transform.position = hit.point;

                        if (ability.inheritPlayerAllegiance)
                            abilityObject.GetComponent<Allegiance>().faction = allegiance.faction;
                    }
                };


                actionBindings.Add(ability, OnAbilityPlayed);
                input.actions.FindAction($"PlayAbility{ability.name}").performed += OnAbilityPlayed;
            }
        }

        /// <summary>
        /// Updates the ability unlock/locked status.
        /// </summary>
        /// <param name="faction"> The faction whose favor has changed. </param>
        /// <param name="god"> The god whose favor has changed. </param>
        private void OnFavorChanged(Faction faction, MinorGod god)
        {
            if (faction != allegiance.faction)
                return;

            foreach(var abilityUnlock in FavorManager.godProgressionSettings[god].abilityUnlocks)
            {
                Ability ability = abilityUnlock.ability;
                // If already unlocked or locked
                if (unlockedAbilities.Contains(ability) || lockedAbilities.Contains(ability))
                    continue;


                // If not enough favor to unlock.
                if (abilityUnlock.favorThreshold > FavorManager.factionToFavor[(faction, god)])
                    continue;


                // Unlock new ability
                int tier = abilityUnlock.abilityTier;

                unlockedAbilities.Add(ability);
                Debug.Log("Unlock");
                if (abilitiesInTiers.Contains(tier))
                {
                    Debug.Log("++");
                    abilitiesInTiers[tier]++;
                }
                else
                {
                    Debug.Log("Add");
                    while (abilitiesInTiers.Count < tier)
                        abilitiesInTiers.Add(0);
                    abilitiesInTiers.Add(1);
                }
                Debug.Log("Invoke");
                onAbilityUnlocked?.Invoke(ability);
                Debug.Log("Done Invoke");

                Debug.Log($"{abilitiesInTiers[tier]} < {FavorManager.progressionSettings.abilitiesPerTier[tier]}");
                // If tier not full
                if (abilitiesInTiers[tier] < FavorManager.progressionSettings.abilitiesPerTier[tier])
                    continue;

                // Lock other abilities in tier
                IEnumerable<AbilityUnlock> newlyLockedAbilities = abilityUnlocks
                        .Where(unlock => unlock.abilityTier == tier && !unlockedAbilities.Contains(unlock.ability));

                foreach (AbilityUnlock lockedAbility in newlyLockedAbilities)
                {
                    lockedAbilities.Add(lockedAbility.ability);
                    onAbilityLocked?.Invoke(lockedAbility.ability);
                }
            }
        }

        private void Update()
        {
            List<Ability> keys = new List<Ability>(abilityCooldowns.Keys);
            foreach (Ability ability in keys)
            {
                _abilityCooldowns[ability] -= Time.deltaTime;
                if (_abilityCooldowns[ability] <= 0)
                {
                    _abilityCooldowns.Remove(ability);
                }
            }
        }

        private void OnDestroy()
        {
            foreach(AbilityUnlock abilityUnlock in abilityUnlocks)
            {
                input.actions.FindAction($"PlayAbility{abilityUnlock.ability.name}").performed -= actionBindings[abilityUnlock.ability];
            }
        }
    }
}