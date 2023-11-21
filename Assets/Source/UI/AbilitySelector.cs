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
        [SerializeField] private RectTransform controllerLayout;


        [SerializeField] private CopyLayout baseContainer;
        [SerializeField] private CopyLayout windContainer;
        [SerializeField] private CopyLayout fireContainer;
        [SerializeField] private CopyLayout waterContainer;
        [SerializeField] private CopyLayout earthContainer;
        
        private AbilityInputHandler manager;

        private void Start()
        {
            manager = GetComponentInParent<AbilityInputHandler>();
            PlayerInput input = GetComponentInParent<PlayerInput>();

            switch (input.currentControlScheme)
            {
                case "Keyboard&Mouse":
                    keyboardLayout.gameObject.SetActive(true);
                    controllerLayout.gameObject.SetActive(false);
                    baseContainer.source = keyboardLayout;
                    windContainer.source = keyboardLayout;
                    waterContainer.source = keyboardLayout;
                    fireContainer.source = keyboardLayout;
                    earthContainer.source = keyboardLayout;
                    break;


                case "Gamepad":
                    keyboardLayout.gameObject.SetActive(false);
                    controllerLayout.gameObject.SetActive(true);
                    baseContainer.source = controllerLayout;
                    windContainer.source = controllerLayout;
                    waterContainer.source = controllerLayout;
                    fireContainer.source = controllerLayout;
                    earthContainer.source = controllerLayout;
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