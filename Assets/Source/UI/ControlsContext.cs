using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ElementalEngagement.UI
{
    public class ControlsContext : MonoBehaviour
    {
        public TextMeshProUGUI template;
        public RectTransform controlsContainer;
        public InputActionReference[] actionsToIgnore;
        private InputManager inputManager;
        private PlayerInput input;
        private List<TextMeshProUGUI> textBoxes = new List<TextMeshProUGUI>();


        private void Awake()
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
                string bindingName = appropriateBindings.Aggregate("", (name, binding) => name + binding.ToDisplayString(control: input.devices[0]));

                textBoxes[index].text = $"{bindingName} - {action.name}";

                index++;
            }


            while (textBoxes.Count >= index)
            {
                Destroy(textBoxes[textBoxes.Count - 1].gameObject);
                textBoxes.RemoveAt(textBoxes.Count - 1);
            }
        }
    }
}