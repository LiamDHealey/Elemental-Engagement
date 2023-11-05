using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ElementalEngagement.UI
{
    public class MenuToggler : MonoBehaviour
    {
        public void Open(string menuName)
        {
            UiInputHandler.onMenuOpened?.Invoke(menuName);
        }
        public void Close(string menuName)
        {
            UiInputHandler.onMenuClosed?.Invoke(menuName);
        }
    }
}