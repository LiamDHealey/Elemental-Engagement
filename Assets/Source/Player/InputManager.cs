using ElementalEngagement.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

namespace ElementalEngagement
{
    [RequireComponent(typeof(PlayerInput), typeof(SelectionManager), typeof(AbilityManager))]
    [RequireComponent(typeof(Panner))]
    public class InputManager : MonoBehaviour
    {
        [Tooltip("The states each action is allowed to be used in")]
        [SerializeField] private List<ActionRequirements> _actionRequirements;
        Dictionary<InputAction, StateFlags> actionRequirements;

        // The input component
        private PlayerInput input;

        // The selection manager
        private SelectionManager selectionManager;

        // The ability manager
        private AbilityManager abilityManager;

        // The ability manager
        private Panner panner;

        private State state = State.Default;

        // Start is called before the first frame update
        private void Awake()
        {
            input = GetComponent<PlayerInput>();
            selectionManager = GetComponent<SelectionManager>();
            abilityManager = GetComponent<AbilityManager>();
            panner = GetComponent<Panner>();

            actionRequirements = _actionRequirements
                .ToDictionary(requirement => requirement.action.action, requirement => requirement.states);
        }

        private bool IsActionAllowed(CallbackContext context)
        {
            return actionRequirements[context.action].HasFlag(state);
        }

        // Update is called once per frame
        private void Start()
        {
            input.actions["Pan"].performed += Pan;
            input.actions["Select"].performed += Select;
            input.actions["CircularSelect"].performed += CircularSelect;
            input.actions["SelectAll"].performed += SelectAll;
            input.actions["DeselectAll"].performed += DeselectAll;
            input.actions["IssueCommand"].performed += IssueCommand;
            input.actions["IssueAltCommand"].performed += IssueAltCommand;
            input.actions["PlayAbility"].performed += PlayAbility;
            input.actions["RotateAbility"].performed += RotateAbility;
            input.actions["SelectAbility0"].performed += SelectAbility0;
            input.actions["SelectAbility1"].performed += SelectAbility1;
            input.actions["SelectAbility2"].performed += SelectAbility2;
            input.actions["SelectAbility3"].performed += SelectAbility3;
            input.actions["PauseGame"].performed += PauseGame;
        }
        private void OnDestroy()
        {
            input.actions["Pan"].performed -= Pan;
            input.actions["Select"].performed -= Select;
            input.actions["CircularSelect"].performed -= CircularSelect;
            input.actions["SelectAll"].performed -= SelectAll;
            input.actions["DeselectAll"].performed -= DeselectAll;
            input.actions["IssueCommand"].performed -= IssueCommand;
            input.actions["IssueAltCommand"].performed -= IssueAltCommand;
            input.actions["PlayAbility"].performed -= PlayAbility;
            input.actions["RotateAbility"].performed -= RotateAbility;
            input.actions["SelectAbility0"].performed -= SelectAbility0;
            input.actions["SelectAbility1"].performed -= SelectAbility1;
            input.actions["SelectAbility2"].performed -= SelectAbility2;
            input.actions["SelectAbility3"].performed -= SelectAbility3;
            input.actions["PauseGame"].performed -= PauseGame;
        }

        #region Bindings
        private void Pan(CallbackContext context)
        {
            if (!IsActionAllowed(context))
                return;

            panner.Pan(context.ReadValue<Vector2>());
        }

        private void Select(CallbackContext context)
        {
            if (!IsActionAllowed(context))
                return;

            selectionManager.Select();
        }

        private void CircularSelect(CallbackContext context)
        {
            if (!IsActionAllowed(context))
                return;

            selectionManager.StartCircularSelection(() => context.action.inProgress);
        }

        private void SelectAll(CallbackContext context)
        {
            if (!IsActionAllowed(context))
                return;

            selectionManager.SelectAll();
        }

        private void DeselectAll(CallbackContext context)
        {
            if (!IsActionAllowed(context))
                return;

        }

        private void IssueCommand(CallbackContext context)
        {
            if (!IsActionAllowed(context))
                return;

            throw new NotImplementedException();
        }

        private void IssueAltCommand(CallbackContext context)
        {
            if (!IsActionAllowed(context))
                return;

            throw new NotImplementedException();
        }

        private void PlayAbility(CallbackContext context)
        {
            if (!IsActionAllowed(context))
                return;

            throw new NotImplementedException();
        }

        private void RotateAbility(CallbackContext context)
        {
            if (!IsActionAllowed(context))
                return;

            throw new NotImplementedException();
        }

        private void SelectAbility0(CallbackContext context)
        {
            throw new NotImplementedException();
        }

        private void SelectAbility1(CallbackContext context)
        {
            if (!IsActionAllowed(context))
                return;

            throw new NotImplementedException();
        }

        private void SelectAbility2(CallbackContext context)
        {
            if (!IsActionAllowed(context))
                return;

            throw new NotImplementedException();
        }

        private void SelectAbility3(CallbackContext context)
        {
            if (!IsActionAllowed(context))
                return;

            throw new NotImplementedException();
        }

        private void PauseGame(CallbackContext context)
        {
            if (!IsActionAllowed(context))
                return;

            throw new NotImplementedException();
        }
        #endregion


        [System.Serializable]
        private class ActionRequirements
        {
            public InputActionReference action;
            public StateFlags states;
        }
        private enum State
        {
            Default,
            UnitsSelected,
            AbilitySelected,
            InMenu,
        }

        [Flags]
        private enum StateFlags
        {
            Default,
            UnitsSelected,
            AbilitySelected,
            InMenu,
        }
    }
}