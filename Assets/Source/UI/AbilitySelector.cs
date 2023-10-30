using ElementalEngagement.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ElementalEngagement.UI
{
    public class AbilitySelector : MonoBehaviour
    {
        [SerializeField] private RectTransform keyboardLayout;
        [SerializeField] private RectTransform controlerLayout;


        [SerializeField] private CopyLayout baseContainer;
        [SerializeField] private CopyLayout windContainer;
        [SerializeField] private CopyLayout waterContainer;
        [SerializeField] private CopyLayout fireContainer;
        [SerializeField] private CopyLayout earthContainer;
        
        private AbilityInputHandler manager;

        private void Start()
        {
            Transform parent = transform.parent;
            while (manager == null && parent != null)
            {
                manager = parent.GetComponent<AbilityInputHandler>();
                parent = parent.parent;
            }
            if (manager == null)
            {
                Debug.LogError("No Ability Manager Found");
                return;
            }

            PlayerInput input = null;
            parent = transform.parent;
            while (input == null && parent != null)
            {
                input = parent.GetComponent<PlayerInput>();
                parent = parent.parent;
            }
            if (input == null)
            {
                Debug.LogError("No Player Input Found");
                return;
            }

            switch (input.currentControlScheme)
            {
                case "Keyboard&Mouse":
                    baseContainer.source = keyboardLayout;
                    windContainer.source = keyboardLayout;
                    waterContainer.source = keyboardLayout;
                    fireContainer.source = keyboardLayout;
                    earthContainer.source = keyboardLayout;
                    break;


                case "Gamepad":
                    baseContainer.source = controlerLayout;
                    windContainer.source = controlerLayout;
                    waterContainer.source = controlerLayout;
                    fireContainer.source = controlerLayout;
                    earthContainer.source = controlerLayout;
                    break;


                default:
                    Debug.LogWarning($"Unknown Control Scheme: {input.currentControlScheme}");
                    break;
            }
        }

        private void Update()
        {
            switch (manager.selectionGod)
            {
                case Favor.MinorGod.Unaligned:
                    baseContainer.gameObject.SetActive(false);
                    windContainer.gameObject.SetActive(true);
                    waterContainer.gameObject.SetActive(false);
                    fireContainer.gameObject.SetActive(false);
                    earthContainer.gameObject.SetActive(false);
                    break;


                case Favor.MinorGod.Fire:
                    baseContainer.gameObject.SetActive(false);
                    windContainer.gameObject.SetActive(false);
                    waterContainer.gameObject.SetActive(false);
                    fireContainer.gameObject.SetActive(true);
                    earthContainer.gameObject.SetActive(false);
                    break;


                case Favor.MinorGod.Water:
                    baseContainer.gameObject.SetActive(false);
                    windContainer.gameObject.SetActive(false);
                    waterContainer.gameObject.SetActive(true);
                    fireContainer.gameObject.SetActive(false);
                    earthContainer.gameObject.SetActive(false);
                    break;


                case Favor.MinorGod.Earth:
                    baseContainer.gameObject.SetActive(false);
                    windContainer.gameObject.SetActive(false);
                    waterContainer.gameObject.SetActive(false);
                    fireContainer.gameObject.SetActive(false);
                    earthContainer.gameObject.SetActive(true);
                    break;


                default:
                    baseContainer.gameObject.SetActive(true);
                    windContainer.gameObject.SetActive(false);
                    waterContainer.gameObject.SetActive(false);
                    fireContainer.gameObject.SetActive(false);
                    earthContainer.gameObject.SetActive(false);
                    break;
            }
        }
    }
}