using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;

namespace ElementalEngagement.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ControlText: MonoBehaviour
    {
        [Tooltip("Replaces {binding} with the button icons this is bound to, and replaces {action} with the name of the action.")]
        public string format = "{binding} - {action}";
        public InputActionReference action;


        private InputManager inputManager;
        private PlayerInput input;
        private TextMeshProUGUI textBox;



        private void Start()
        {
            inputManager = GetComponentInParent<InputManager>();
            input = GetComponentInParent<PlayerInput>();
            textBox = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            IEnumerable<InputBinding> appropriateBindings = action.action.bindings.Where(b => b.groups.Contains(input.currentControlScheme));

            string bindingName = appropriateBindings
                    .Aggregate("", (name, binding) =>
                    {
                        return name + $"<sprite name=\"{binding.ToDisplayString(InputBinding.DisplayStringOptions.DontIncludeInteractions, control: input.devices[0])}\">";
                    });
            string bindingInteractions = appropriateBindings.First().interactions.Split('(')[0];
            textBox.text = format.Replace("{binding}", $"{bindingInteractions} {bindingName}").Replace("{action}", action.name);
        }
    }
}