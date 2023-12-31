using ElementalEngagement.Combat;
using ElementalEngagement.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

namespace ElementalEngagement
{
    [RequireComponent(typeof(PlayerInput), typeof(SelectionInputHandler), typeof(AbilityInputHandler))]
    [RequireComponent(typeof(CameraMovementHandler))]
    public class InputManager : MonoBehaviour
    {
        [Tooltip("The states each action is allowed to be used in")]
        [SerializeField] private List<ActionRequirements> _actionRequirements;
        private Dictionary<string, ActionRequirements> actionRequirements;


        public IEnumerable<InputAction> availableActions => input.actions.Where(IsActionAllowed);


        // The input component
        private PlayerInput input;

        // The selection manager
        private SelectionInputHandler selectionInputHandler;

        // The ability manager
        private AbilityInputHandler abilityInputHandler;

        // The ability manager
        private CameraMovementHandler panInputHandler;

        private State? _state = null;
        private State state
        {
            get
            {
                if (_state != null)
                    return _state.Value;


                if (UIManager.isUIOpen)
                    _state = State.InMenu;
                else if (abilityInputHandler.isAbilitySelected)
                    if (abilityInputHandler.selectedAbility.canBeRotated)
                        _state = State.AbilitySelected | State.RotatingAbility;
                    else
                        _state = State.AbilitySelected;
                else if (abilityInputHandler.isSelectionInProgress)
                    if (selectionInputHandler.selectedObjects.Count > 0)
                        _state = State.SelectingAbility | State.UnitsSelected;
                    else
                        _state = State.SelectingAbility;
                else if (selectionInputHandler.selectedObjects.Count > 0)
                {
                    _state = selectionInputHandler.GetCurrentCommand() switch
                    {
                        SacrificeCommand => State.UnitsSelected | State.HoveringOverShrine,
                        ChaseCommand => State.UnitsSelected | State.HoveringOverEnemies,
                        _ => State.UnitsSelected,
                    };
                }
                else
                    _state = State.Default;


                return _state.Value;
            }
        }



        // Start is called before the first frame update
        private void Awake()
        {
            input = GetComponent<PlayerInput>();
            selectionInputHandler = GetComponent<SelectionInputHandler>();
            abilityInputHandler = GetComponent<AbilityInputHandler>();
            panInputHandler = GetComponent<CameraMovementHandler>();

            actionRequirements = _actionRequirements
                .ToDictionary(requirement => requirement.action.action.name);
        }

        private bool IsActionAllowed(CallbackContext context)
        {
            return IsActionAllowed(context.action);
        }

        private bool IsActionAllowed(InputAction action)
        {
            if (!actionRequirements.ContainsKey(action.name))
                return false;
            ActionRequirements req = actionRequirements[action.name];
            return (req.requiredStates == 0 || (req.requiredStates & state) > 0) &&
                   (req.forbiddenStates & state) == 0;
        }

        // Update is called once per frame
        private void Start()
        {
            input.actions["Select"].performed += Select;
            input.actions["CircularSelect"].performed += CircularSelect;
            input.actions["SelectAll"].performed += SelectAll;
            input.actions["DeselectAll"].performed += DeselectAll;
            input.actions["Move"].performed += IssueCommand;
            input.actions["Attack"].performed += IssueCommand;
            input.actions["Sacrifice"].performed += IssueCommand;
            input.actions["AttackMove"].performed += IssueAltCommand;
            input.actions["PlayAbility"].performed += PlayAbility;
            input.actions["SelectAbility0"].performed += SelectAbility0;
            input.actions["SelectAbility1"].performed += SelectAbility1;
            input.actions["SelectAbility2"].performed += SelectAbility2;
            input.actions["SelectAbility3"].performed += SelectAbility3;
            input.actions["PauseGame"].performed += PauseGame;
            input.actions["Back"].performed += Back;
            input.actions["SelectEverything"].performed += SelectEverything;
            input.actions["Stop"].performed += Stop;
        }

        private void OnDestroy()
        {
            input.actions["Select"].performed -= Select;
            input.actions["CircularSelect"].performed -= CircularSelect;
            input.actions["SelectAll"].performed -= SelectAll;
            input.actions["DeselectAll"].performed -= DeselectAll;
            input.actions["Move"].performed -= IssueCommand;
            input.actions["Attack"].performed -= IssueCommand;
            input.actions["Sacrifice"].performed -= IssueCommand;
            input.actions["AttackMove"].performed -= IssueAltCommand;
            input.actions["PlayAbility"].performed -= PlayAbility;
            input.actions["SelectAbility0"].performed -= SelectAbility0;
            input.actions["SelectAbility1"].performed -= SelectAbility1;
            input.actions["SelectAbility2"].performed -= SelectAbility2;
            input.actions["SelectAbility3"].performed -= SelectAbility3;
            input.actions["PauseGame"].performed -= PauseGame;
            input.actions["Back"].performed -= Back;
            input.actions["SelectEverything"].performed -= SelectEverything;
            input.actions["Stop"].performed -= Stop;
        }

        private void Update()
        {
            _state = null;

            if (input.actions["ZoomIn"].inProgress)
            {
                ZoomIn(input.actions["ZoomIn"]);
            }
            else if (input.actions["ZoomOut"].inProgress)
            {
                ZoomOut(input.actions["ZoomOut"]);
            }
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

        private void SelectEverything(CallbackContext context)
        {
            if (!IsActionAllowed(context))
                return;

            selectionInputHandler.SelectEverything();
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

        private void Stop(CallbackContext context)
        {
            if (!IsActionAllowed(context))
                return;

            selectionInputHandler.StopSelectedCommands();
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

            abilityInputHandler.RotateAbility(action.ReadValue<float>());
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

            if (!UIManager.IsMenuOpen("pauseMenu"))
            {
                UIManager.onMenuOpened?.Invoke("pauseMenu");
            }
            else
            {
                UIManager.onMenuClosed?.Invoke("pauseMenu");
            }
        }

        private void Back(CallbackContext context)
        {
            if (!IsActionAllowed(context))
                return;

            if (state.HasFlag(State.SelectingAbility) || state.HasFlag(State.AbilitySelected))
            {
                abilityInputHandler.UndoSelection();
            }
            else if (state.HasFlag(State.InMenu))
            {
                UIManager.onMenuClosed?.Invoke("pauseMenu");
            }
        }

        void ZoomIn(InputAction action)
        {
            if (!IsActionAllowed(action))
                return;
            panInputHandler.Zoom(-new Vector2(0, action.ReadValue<float>()));
        }

        void ZoomOut(InputAction action)
        {
            if (!IsActionAllowed(action))
                return;

            panInputHandler.Zoom(new Vector2(0, action.ReadValue<float>()));
        }

        #endregion


        [System.Serializable]
        private class ActionRequirements
        {
            public InputActionReference action;
            public State requiredStates;
            public State forbiddenStates;
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
            RotatingAbility = 32,
            HoveringOverEnemies = 64,
            HoveringOverShrine = 128,
        }
    }
}