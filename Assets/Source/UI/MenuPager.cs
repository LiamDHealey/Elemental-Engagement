using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ElementalEngagement.UI
{
    public class MenuPager : MonoBehaviour
    {

        [SerializeField] private Button nextButton;
        [SerializeField] private Button prevButton;
        [SerializeField] private GameObject altNextButton;
        [SerializeField] private GameObject altPrevButton;
        [SerializeField] private List<GameObject> menus;

        [SerializeField] private int _menuIndex;
        public int menuIndex
        {
            get => _menuIndex;
            set
            {
                if (!gameObject.activeInHierarchy)
                    return;

                if (value >= menus.Count)
                    throw new ArgumentOutOfRangeException();

                menus[_menuIndex].SetActive(false);
                menus[value].SetActive(true);
                int oldIndex = _menuIndex;
                _menuIndex = value;
                onMenuIndexChanged?.Invoke(oldIndex);
            }
        }


        public UnityEvent<int> onMenuIndexChanged;

        private void Start()
        {
            foreach (GameObject menu in menus)
            {
                menu.SetActive(false);
            }
            menuIndex = menuIndex;

            nextButton.onClick.AddListener(() => menuIndex++);
            prevButton.onClick.AddListener(() => menuIndex--);
        }

        private void Update()
        {
            nextButton.gameObject.SetActive(menuIndex != menus.Count - 1 && nextButton.transform.parent.gameObject.activeSelf);
            if (altNextButton != null)
            {
                altNextButton.SetActive(menuIndex == menus.Count - 1);
            }
            prevButton.gameObject.SetActive(menuIndex != 0);
            if (altPrevButton != null)
                altPrevButton.SetActive(menuIndex == 0 && nextButton.transform.parent.gameObject.activeSelf);
        }
    }
}