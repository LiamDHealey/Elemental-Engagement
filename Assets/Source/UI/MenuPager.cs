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
        [SerializeField] private bool isTabbedList;


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
            if(isTabbedList) 
            {
                //TODO: Create a template game object that holds a button. Set that button's listener with the following lambda
                //Also, Add a menu pager to each of the Menu Containers in Unity that can then cycle through the pages

                for (int i = 0; i < 10; i++)
                {
                    GetComponent<Button>().onClick.AddListener(() => menuIndex = i);
                }
            }
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