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
    [RequireComponent(typeof(PlayerInput), typeof(SelectionInputHandler), typeof(AbilityInputHandler))]
    [RequireComponent(typeof(PanInputHandler), typeof(UiInputHandler))]
    public class InputManager : MonoBehaviour
    {
        [Tooltip("The states each action is allowed to be used in")]
        [SerializeField] private List<ActionRequirements> _actionRequirements;
        private Dictionary<string, State> actionRequirements;


        public IEnumerable<InputAction> availableActions => input.actions.Where(IsActionAllowed);


        // The input component
        private PlayerInput input;

        // The selection manager
        private SelectionInputHandler selectionInputHandler;

        // The ability manager
        private AbilityInputHandler abilityInputHandler;

        // The ability manager
        private PanInputHandler panInputHandler;

        // The UI manager
        private UiInputHandler uiInputHandler;

        private State state
        {
            get
            {
                if (uiInputHandler.isUIOpen)
                    return State.InMenu;
                if (abilityInputHandler.isAbilitySelected)
                        return State.AbilitySelected;
                if (abilityInputHandler.isSelectionInProgress)
                    if (selectionInputHandler.selectedObjects.Count > 0)
                        return State.SelectingAbility | State.UnitsSelected;
                    else
                        return State.SelectingAbility;
                if (selectionInputHandler.selectedObjects.Count > 0)
                    return State.UnitsSelected;
                else
                    return State.Default;
            }
        }

        // Start is called before the first frame update
        private void Awake()
        {
            input = GetComponent<PlayerInput>();
            selectionInputHandler = GetComponent<SelectionInputHandler>();
            abilityInputHandler = GetComponent<AbilityInputHandler>();
            panInputHandler = GetComponent<PanInputHandler>();
            uiInputHandler = GetComponent<UiInputHandler>();

            actionRequirements = _actionRequirements
                .ToDictionary(requirement => requirement.action.action.name, requirement => requirement.states);
        }

        private bool IsActionAllowed(CallbackContext context)
        {
            return IsActionAllowed(context.action);
        }

        private bool IsActionAllowed(InputAction action)
        {
            if (!actionRequirements.ContainsKey(action.name))
                return false;
            return actionRequirements[action.name].HasFlag((State)state);
        }

        // Update is called once per frame
        private void Start()
        {
            input.actions["Select"].performed += Select;
            input.actions["CircularSelect"].performed += CircularSelect;
            input.actions["SelectAll"].performed += SelectAll;
            input.actions["DeselectAll"].performed += DeselectAll;
            input.actions["IssueCommand"].performed += IssueCommand;
            input.actions["IssueAltCommand"].performed += IssueAltCommand;
            input.actions["PlayAbility"].performed += PlayAbility;
            input.actions["SelectAbility0"].performed += SelectAbility0;
            input.actions["SelectAbility1"].performed += SelectAbility1;
            input.actions["SelectAbility2"].performed += SelectAbility2;
            input.actions["SelectAbility3"].performed += SelectAbility3;
            input.actions["PauseGame"].performed += PauseGame;
            input.actions["Back"].performed += Back;
        }

        private void OnDestroy()
        {
            input.actions["Select"].performed -= Select;
            input.actions["CircularSelect"].performed -= CircularSelect;
            input.actions["SelectAll"].performed -= SelectAll;
            input.actions["DeselectAll"].performed -= DeselectAll;
            input.actions["IssueCommand"].performed -= IssueCommand;
            input.actions["IssueAltCommand"].performed -= IssueAltCommand;
            input.actions["PlayAbility"].performed -= PlayAbility;
            input.actions["SelectAbility0"].performed -= SelectAbility0;
            input.actions["SelectAbility1"].performed -= SelectAbility1;
            input.actions["SelectAbility2"].performed -= SelectAbility2;
            input.actions["SelectAbility3"].performed -= SelectAbility3;
            input.actions["PauseGame"].performed -= PauseGame;
            input.actions["Back"].performed -= Back;
        }

        private void Update()
        {
            Pan(input.actions["Pan"]);
            RotateAbility(input.actions["RotateAbility"]);
        }

        #region Bindings
        void Pan(InputAction action)
        {
            if (!IsActionAllowed(action))
                return;

            panInputHandler.Pan(action.ReadValue<Vector2>());
        }

        private void Select(CallbackContext context)
        {
            if (!IsActionAllowed(context))
                return;

            selectionInputHandler.Select();
        }

        private void CircularSelect(CallbackContext context)
        {
            if (!IsActionAllowed(context))
                return;

            selectionInputHandler.StartCircularSelection(() => context.action.inProgress);
        }

        private void SelectAll(CallbackContext context)
        {
            if (!IsActionAllowed(context))
                return;

            selectionInputHandler.SelectAll();
        }

        private void DeselectAll(CallbackContext context)
        {
            if (!IsActionAllowed(context))
                return;

            selectionInputHandler.DeselectAll();
        }

        private void IssueCommand(CallbackContext context)
        {
            if (!IsActionAllowed(context))
                return;

            selectionInputHandler.IssueCommand(false);
        }

        private void IssueAltCommand(CallbackContext context)
        {
            if (!IsActionAllowed(context))
                return;

            selectionInputHandler.IssueCommand(true);
        }

        private void PlayAbility(CallbackContext context)
        {
            if (!IsActionAllowed(context))
                return;

            abilityInputHandler.PlayAbility();
        }

        private void RotateAbility(InputAction action)
        {
            if (!IsActionAllowed(action))
                return;

            abilityInputHandler.RotateAbility(action.ReadValue<Vector2>());
        }

        private void SelectAbility0(CallbackContext context)
        {
            if (!IsActionAllowed(context))
                return;

            abilityInputHandler.SelectAbility(0);
        }

        private void SelectAbility1(CallbackContext context)
        {
            if (!IsActionAllowed(context))
                return;

            abilityInputHandler.SelectAbility(1);
        }

        private void SelectAbility2(CallbackContext context)
        {
            if (!IsActionAllowed(context))
                return;

            abilityInputHandler.SelectAbility(2);
        }

        private void SelectAbility3(CallbackContext context)
        {
            if (!IsActionAllowed(context))
                return;

            abilityInputHandler.SelectAbility(3);
        }

        private void PauseGame(CallbackContext context)
        {
            if (!IsActionAllowed(context))
                return;

            uiInputHandler.Pause();
        }

        private void Back(CallbackContext context)
        {
            if (!IsActionAllowed(context))
                return;

            switch (state)
            {
                case State.SelectingAbility:
                    abilityInputHandler.ResetSelection();
                    break;
                case State.AbilitySelected:
                    abilityInputHandler.ResetSelection();
                    break;
                case State.InMenu:
                    uiInputHandler.Back();
                    break;
            }
        }
        #endregion


        [System.Serializable]
        private class ActionRequirements
        {
            public InputActionReference action;
            public State states;
        }

        [Flags]
        private enum State
        {
            None = 0,
            Default = 1,
            UnitsSelected = 2,
            SelectingAbility = 4,
            AbilitySelected = 8,
            InMenu = 16,
        }
    }
}