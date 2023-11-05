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

    public static UnityEvent<string> onMenuOpened = new UnityEvent<string>();

    public static UnityEvent<string> onMenuClosed = new UnityEvent<string>();

    public bool isUIOpen { get; private set; } = false;

    public void Pause()
    {
        onMenuOpened?.Invoke("pauseMenu");
        Time.timeScale = 0f;
        isUIOpen = true;
    }

    public void Back()
    {
        onMenuClosed?.Invoke("pauseMenu");
        Time.timeScale = 1f;
        isUIOpen = false;
    }

    public void OpenGuide()
    {

    }

    public void GoToMainMenu()
    {
        SceneManager.LoadSceneAsync("Content/Scenes/Main Menu");
    }
}
