using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class UiInputHandler : MonoBehaviour
{
    [Tooltip("The input component that this will get mouse data from.")]
    [SerializeField] private PlayerInput input;

    [SerializeField] GameObject pauseMenu;

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Back()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OpenGuide()
    {

    }
}
