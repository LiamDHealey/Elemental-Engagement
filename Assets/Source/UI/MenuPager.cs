using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ElementalEngagement.UI
{
    public class MenuPager : MonoBehaviour
    {

        [SerializeField] private Button nextButton;
        [SerializeField] private Button prevButton;
        [SerializeField] private List<GameObject> menus;


        [SerializeField] private int _menuIndex;
        public int menuIndex
        {
            get => _menuIndex;
            set
            {
                if (value >= menus.Count)
                    throw new ArgumentOutOfRangeException();

                menus[_menuIndex].SetActive(false);
                if (_menuIndex == 0)
                {
                    prevButton.gameObject.SetActive(true);
                }
                else if (_menuIndex == menus.Count - 1)
                {
                    nextButton.gameObject.SetActive(true);
                }


                _menuIndex = value;


                menus[_menuIndex].SetActive(true);
                if (_menuIndex == 0)
                {
                    prevButton.gameObject.SetActive(false);
                }
                else if (_menuIndex == menus.Count - 1)
                {
                    nextButton.gameObject.SetActive(false);
                }
            }
        }


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
    }
}