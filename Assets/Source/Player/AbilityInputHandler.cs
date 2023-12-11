using ElementalEngagement.Combat;
using ElementalEngagement.Favor;
using ElementalEngagement.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
    public class AbilityInputHandler : MonoBehaviour
    {
        [Tooltip("Called when the selected ability changes.")]
        public UnityEvent<Ability> onSelectedAbilityChanged;

        public UnityEvent onAbilitySelected;

        public UnityEvent onAbilityDeselected;

        [Tooltip("Called whenever the ability menu is navigated through")]
        public UnityEvent onAbilityMenuChanged;

        [Tooltip("Called when an ability was played.")]
        public UnityEvent<Ability> onAbilityPlayed;

        [Tooltip("Called when an ability is unlocked.")]
        public UnityEvent<Ability> onAbilityUnlocked;

        [Tooltip("Called when an ability is locked.")]
        public UnityEvent<Ability> onAbilityLocked;

        [Tooltip("The input to get the play bindings from.")]
        [SerializeField] private PlayerInput input;

        [Tooltip("The speed at which abilities are rotated.")]
        public float rotationSpeed = 180;

        // Whether or not an ability is currently selected.
        public bool isAbilitySelected => currentSelection[0] != null && currentSelection[1] != null;

        // Whether or not an ability is in the process of being selected.
        public bool isSelectionInProgress => currentSelection[0] != null && currentSelection[1] == null;

        // Whether or not an ability is currently selected.
        public MinorGod? selectionGod => currentSelection[0] switch
        {
            0 => MinorGod.Unaligned,
            1 => MinorGod.Fire,
            2 => MinorGod.Water,
            3 => MinorGod.Earth,
            _ => null,
        };
            

        // The ability that is currently selected
        public Ability selectedAbility => isAbilitySelected ? abilities[currentSelection[0].Value, currentSelection[1].Value] : null;

        // The current abilities' remaining cooldown times in seconds.
        private Dictionary<Ability, float> _abilityCooldowns = new Dictionary<Ability, float>();
        public ReadOnlyDictionary<Ability, float> abilityCooldowns { get => new ReadOnlyDictionary<Ability, float>(_abilityCooldowns); }

        // The allegaince of this.
        public Allegiance allegiance { get; private set; }



        // All the abilities available in this game.
        private HashSet<AbilityUnlock> abilityUnlocks;

        // All the abilities mapped to thier input paths.
        private Ability[,] abilities = new Ability[4,4];

        // The abilities that have been unlocked.
        private HashSet<Ability> unlockedAbilities;

        // The abilities that have been locked.
        private HashSet<Ability> lockedAbilities = new HashSet<Ability>();

        // The number of unlocked abilities in each tier.
        private List<int> _abilitiesInTiers = new List<int>();
        public ReadOnlyCollection<int> abilitiesInTiers;

        // The currently selected abilities.
        private readonly int?[] currentSelection = { null, null };

        // The currently selected ability's preview.
        private GameObject abilityPreview;

        private void Awake() => allegiance = GetComponent<Allegiance>();

        private void Start()
        {
            FavorManager.onFavorChanged.AddListener(OnFavorChanged);
            abilitiesInTiers = new ReadOnlyCollection<int>(_abilitiesInTiers);


            abilityUnlocks = FavorManager.godProgressionSettings.Values
                .SelectMany(setting => setting.abilityUnlocks)
                .ToHashSet();
            foreach (AbilityUnlock abilityUnlock in abilityUnlocks)
            {
                Ability ability = abilityUnlock.ability;
                abilities[ability.menuIndex, ability.submenuIndex] = ability;
            }



            unlockedAbilities = FavorManager.godProgressionSettings.Values
                .SelectMany(setting => setting.abilityUnlocks)
                .Where(unlock => unlock.favorThreshold <= 0)
                .Select(unlock => unlock.ability)
                .ToHashSet();
            foreach (Ability unlockedAbility in unlockedAbilities)
            {
                onAbilityUnlocked?.Invoke(unlockedAbility);
                _abilityCooldowns.Add(unlockedAbility, unlockedAbility.cooldown);
            }
        }


        public SelectionResult SelectAbility(int index)
        {
            onAbilityMenuChanged?.Invoke();
            if (currentSelection[0] == null)
            {
                currentSelection[0] = index;
                return SelectionResult.SubmenuOpened;
            }

            Ability ability = abilities[currentSelection[0].Value, index];
            if (unlockedAbilities.Contains(ability))
            {
                int? lastSelection = currentSelection[1];
                currentSelection[1] = index;
                onAbilitySelected?.Invoke();

                if (lastSelection == index)
                {
                    PlayAbility();
                    return SelectionResult.AbilityPlayed;
                }

                if (abilityPreview != null)
                {
                    Destroy(abilityPreview.gameObject);
                }

                abilityPreview = Instantiate(ability.previewPrefab);
                abilityPreview.transform.SetParent(transform);
                if (ability.inheritPlayerAllegiance && abilityPreview.GetComponent<Allegiance>() != null)
                    abilityPreview.GetComponent<Allegiance>().faction = allegiance.faction;

                abilityPreview.transform.position = MathHelpers.IntersectWithGround(new Ray(transform.position, transform.forward));
                return SelectionResult.Success;
            }

            return SelectionResult.AbilityNotAvailable;
        }

        public void FixedUpdate()
        {
            if (abilityPreview != null)
            {
                abilityPreview.transform.position = MathHelpers.IntersectWithGround(new Ray(transform.position, transform.forward));
            }
        }

        public enum SelectionResult { SubmenuOpened, Success, AbilityNotAvailable, AbilityPlayed }

        public void RotateAbility(float rotationAmount)
        {
            if (Math.Abs(rotationAmount) < Vector2.kEpsilon)
                return;

            if (abilityPreview == null)
                return;

            if (!selectedAbility.canBeRotated)
                return;

            abilityPreview.transform.right = Quaternion.AngleAxis(rotationAmount * rotationSpeed * Time.deltaTime 
                                                                  + abilityPreview.transform.rotation.eulerAngles.y, Vector3.up)
                                             * Vector3.right;
        }

        public void ResetSelection()
        {
            onAbilityDeselected?.Invoke();
            currentSelection[0] = null;
            currentSelection[1] = null;

            if (abilityPreview != null)
            {
                Destroy(abilityPreview.gameObject);
            }
        }

        public void UndoSelection()
        {
            onAbilityDeselected?.Invoke();
            if (currentSelection[1] == null)
            {
                currentSelection[0] = null;
            }
            currentSelection[1] = null;

            if (abilityPreview != null)
            {
                Destroy(abilityPreview.gameObject);
            }
        }

        public void PlayAbility()
        {
            Ability ability = selectedAbility;

            if (!unlockedAbilities.Contains(ability) || _abilityCooldowns.ContainsKey(ability))
                return;

            if (ability.hasCollision && (abilityPreview.GetComponent<IAbilityCollider>()?.isColliding ?? false))
                return;

            _abilityCooldowns.Add(ability, ability.cooldown);

            GameObject abilityObject = Instantiate(ability.abilityPrefab);
            abilityObject.transform.position = abilityPreview.transform.position;
            abilityObject.transform.rotation = abilityPreview.transform.rotation;

            if (ability.inheritPlayerAllegiance)
                abilityObject.GetComponent<Allegiance>().faction = allegiance.faction;
            
            ResetSelection();
        }

        public bool IsAbilityUnlocked(Ability ability) => unlockedAbilities?.Contains(ability) ?? false;
        public bool IsAbilityLocked(Ability ability) => lockedAbilities?.Contains(ability) ?? false;

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


                while (_abilitiesInTiers.Count <= tier)
                    _abilitiesInTiers.Add(0);
                _abilitiesInTiers[tier]++;
                unlockedAbilities.Add(ability);
                onAbilityUnlocked?.Invoke(ability);

                // If tier not full
                if (_abilitiesInTiers[tier] < FavorManager.progressionSettings.abilitiesPerTier[tier])
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
    }
}