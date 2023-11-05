using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;

public class UiInputHandler : MonoBehaviour
{
    [Tooltip("The input component that this will get mouse data from.")]
    [SerializeField] private PlayerInput input;

    [SerializeField] GameObject pauseMenu;

    private int openMenus = 1;

    public static UnityEvent<string> onMenuOpened = new UnityEvent<string>();

    public static UnityEvent<string> onMenuClosed = new UnityEvent<string>();

    public bool isUIOpen { get; private set; } = false;

    private void Start()
    {
        onMenuOpened.AddListener(menuOpened);
        onMenuClosed.AddListener(menuClosed);
    }

    private void menuOpened(string name)
    {
        openMenus++;
    }

    private void menuClosed(string name)
    {
        openMenus--;
    }

    private void Update()
    {
        if(openMenus > 0)
        {
            Time.timeScale = 0f;
        }

        else
        {
            Time.timeScale = 1.0f;
        }
    }


}
