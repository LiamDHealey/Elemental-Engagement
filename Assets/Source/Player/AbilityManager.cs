using ElementalEngagement.Combat;
using ElementalEngagement.Favor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
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
        public UnityEvent<int> onSelectedAbilityChanged;

        [Tooltip("Called when an ability was played. Passes the ability index")]
        public UnityEvent<int> onAbilityPlayed;

        [Tooltip("Called when an ability comes off cooldown. Passes the ability index.")]
        public UnityEvent<int> onAbilityCooldownExpired;

        [Tooltip("The cursor used to get the ability play location from.")]
        [SerializeField] private PlayerCursor cursor;

        [Tooltip("The input to get the play bindings from.")]
        [SerializeField] private PlayerInput input;

        // The currently selected ability
        public int selectedAbilityIndex { get; private set; }


        // The current abilities' remaining cooldown times in seconds.
        private Dictionary<Ability, float> _abilityCooldowns = new Dictionary<Ability, float>();
        public ReadOnlyDictionary<Ability, float> abilityCooldowns { get => new ReadOnlyDictionary<Ability, float>(_abilityCooldowns); }


        private List<Action<CallbackContext>> actionBindings;

        private void Start()
        {
            actionBindings = new List<Action<CallbackContext>>();
            int i = 0;
            InputAction action = input.actions.FindAction($"PlayAbility{i + 1}");
            while (action != null)
            {
                int abilityIndex = i;
                // Play ability
                actionBindings.Add(delegate
                {
                    if (FavorManager.unlockedAbilities.Count <= abilityIndex)
                        return;

                    Ability ability = FavorManager.unlockedAbilities[abilityIndex];
                    if (abilityCooldowns.ContainsKey(ability))
                        return;


                    _abilityCooldowns.Add(ability, ability.cooldown);
                    if (cursor.RayUnderCursor(out Ray ray) && Physics.Raycast(ray, out RaycastHit hit))
                    {
                        GameObject abilityObject = Instantiate(ability.prefabToSpawn);
                        abilityObject.transform.position = hit.point;

                        if (ability.inheritPlayerAllegiance)
                            abilityObject.GetComponent<Allegiance>().faction = GetComponent<Allegiance>().faction;
                    }
                });



                action.performed += actionBindings[i];


                action = input.actions.FindAction($"PlayAbility{++i + 1}");
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
            for (int i = 0; i < actionBindings.Count; i++)
            {
                input.actions.FindAction($"PlayAbility{++i + 1}").performed -= actionBindings[i];
            }
        }
    }
}