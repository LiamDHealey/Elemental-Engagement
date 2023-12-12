using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [Tooltip("The name of the menu this represents")]
    [SerializeField] private string _menuName;
    public string menuName => _menuName;

    public bool isOpen => transform.GetChild(0).gameObject.activeSelf;


    private Selectable lastSelectedObject;

    void Awake()
    {
        UIManager.onMenuOpened.AddListener(OpenMenu);
        UIManager.onMenuClosed.AddListener(CloseMenu);
        UIManager.onMenuFocused.AddListener(FocusMenu);
    }

    private void Update()
    {
        if (!UIManager.usingNavigation)
            return;

        if (UIManager.openMenus.FindLast(_ => true) != _menuName)
            return;

        if (lastSelectedObject == null)
        {
            lastSelectedObject = GetComponentInChildren<Selectable>();
            lastSelectedObject.Select();
        }

        if (!lastSelectedObject.gameObject.activeInHierarchy
            || !lastSelectedObject.interactable)
        {
            Selectable closestSelectable = null;

            foreach (Selectable selectable in GetComponentsInChildren<Selectable>())
            {
                if (selectable == lastSelectedObject)
                    continue;

                if (closestSelectable == null)
                {
                    closestSelectable = selectable;
                    continue;
                }

                if (DistanceToLastSelection(closestSelectable) > DistanceToLastSelection(selectable))
                {
                    closestSelectable = selectable;
                    continue;
                }
            }

            lastSelectedObject = closestSelectable;
            lastSelectedObject.Select();

            float DistanceToLastSelection(Selectable selectable)
            {
                return (selectable.transform.position - lastSelectedObject.transform.position).sqrMagnitude;
            }
        }
        else
        {
            lastSelectedObject = EventSystem.current.currentSelectedGameObject?.GetComponent<Selectable>();
        }
    }

    private void FocusMenu(string menuName)
    {
        if (menuName == _menuName)
        {
            lastSelectedObject = GetComponentInChildren<Selectable>();
            lastSelectedObject.Select();
        }
    }

    private void OpenMenu(string menuName)
    {
        if(menuName == _menuName)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            if (UIManager.usingNavigation)
                FocusMenu(menuName);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    private void CloseMenu(string menuName) 
    {
        if (menuName == _menuName)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        else if (UIManager.openMenus.FindLast(menu => menu != menuName) == _menuName)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
