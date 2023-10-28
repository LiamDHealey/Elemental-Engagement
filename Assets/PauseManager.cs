using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PauseManager : MonoBehaviour
{
    [Tooltip("The input component that this will get mouse data from.")]
    [SerializeField] private PlayerInput input;

    [SerializeField] GameObject pauseMenu;
    void Start()
    {
        input.actions["PauseGame"].performed += pause;
    }

    public void pause(CallbackContext context)
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void back()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void openGuide()
    {

    }
}
