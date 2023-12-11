using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ElementalEngagement.UI
{
    public class PagerTab : MonoBehaviour
    {
        public MenuPager menuPager;
        public int index;

        private Button button;

        // Start is called before the first frame update
        void Start()
        {
            button = GetComponent<Button>();
            menuPager.onMenuIndexChanged.AddListener(_ => button.interactable = menuPager.menuIndex != index);
            button.interactable = menuPager.menuIndex != index;

            button.onClick.AddListener(() => menuPager.menuIndex = index);
        }

    } 
}
