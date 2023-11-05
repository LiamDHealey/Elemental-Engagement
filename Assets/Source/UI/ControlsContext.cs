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
    public class ControlsContext : MonoBehaviour
    {
        [Tooltip("Replaces {binding} with the button icons this is bound to, and replaces {action} with the name of the action.")]
        public string format = "{binding} - {action}";
        public TextMeshProUGUI template;
        public RectTransform controlsContainer;
        public InputActionReference[] actionsToIgnore;
        public TMP_SpriteAsset keyboardSpriteAsset;
        public TMP_SpriteAsset playStationSpriteAsset;
        public TMP_SpriteAsset xBoxSpriteAsset;


        private InputManager inputManager;
        private PlayerInput input;
        private List<TextMeshProUGUI> textBoxes = new List<TextMeshProUGUI>();


        private void Start()
        {
            inputManager = GetComponentInParent<InputManager>();
            input = GetComponentInParent<PlayerInput>();
            template.gameObject.SetActive(false);
        }

        private void Update()
        {
            IEnumerable<InputAction> actions = inputManager.availableActions;
            int index = 0;
            foreach (InputAction action in actions)
            {
                if (actionsToIgnore.Any(actionRef => actionRef.action.name == action.name))
                    continue;

                if (textBoxes.Count <= index)
                {
                    GameObject newTextBox = Instantiate(template.gameObject);
                    newTextBox.transform.SetParent(controlsContainer.transform, false);
                    newTextBox.SetActive(true);
                    textBoxes.Add(newTextBox.GetComponent<TextMeshProUGUI>());
                }


                


                IEnumerable<InputBinding> appropriateBindings = action.bindings.Where(b => b.groups.Contains(input.currentControlScheme));

                string bindingName;
                if (input.devices.Any(device => device is DualShockGamepad))
                {
                    textBoxes[index].spriteAsset = playStationSpriteAsset;
                    bindingName = GetBindingNameWithIcons();
                }
                else if (input.devices.Any(device => device is XInputController))
                {
                    textBoxes[index].spriteAsset = xBoxSpriteAsset;
                    bindingName = GetBindingNameWithIcons();
                }
                else if (input.devices.Any(d => d is Keyboard))
                {
                    textBoxes[index].spriteAsset = keyboardSpriteAsset;
                    bindingName = GetBindingNameWithIcons();
                }
                else
                {
                    bindingName = GetBindingName();
                }

                string bindingInteractions = appropriateBindings.First().interactions.Split('(')[0];
                textBoxes[index].text = format.Replace("{binding}", $"{bindingInteractions} {bindingName}").Replace("{action}", action.name);

                index++;





                string GetBindingNameWithIcons()
                {
                    return appropriateBindings
                        .Aggregate("", (name, binding) =>
                        {
                            return name + $"<sprite name=\"{binding.ToDisplayString(InputBinding.DisplayStringOptions.DontIncludeInteractions, control: input.devices[0])}\">";
                        });
                }
                string GetBindingName()
                {
                    if (input.devices.Count == 0)
                        return appropriateBindings
                            .Aggregate("", (name, binding) =>
                            {
                                return name + binding.ToDisplayString(InputBinding.DisplayStringOptions.DontIncludeInteractions);
                            });

                    return appropriateBindings
                        .Aggregate("", (name, binding) =>
                        {
                            return name + binding.ToDisplayString(InputBinding.DisplayStringOptions.DontIncludeInteractions, control: input.devices[0]);
                        });
                }
            }


            while (textBoxes.Count > index)
            {
                Destroy(textBoxes[textBoxes.Count - 1].gameObject);
                textBoxes.RemoveAt(textBoxes.Count - 1);
            }
        }
    }
}