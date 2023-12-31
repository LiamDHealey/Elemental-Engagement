using ElementalEngagement.Player;
using ElementalEngagement.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.StandaloneInputModule;
using static UnityEngine.InputSystem.InputAction;

public class UIManager : MonoBehaviour
{
    public static bool isUIOpen => openMenus.Count > 0;
    public static UnityEvent<string> onMenuOpened = new UnityEvent<string>();
    public static UnityEvent<string> onMenuClosed = new UnityEvent<string>();
    public static UnityEvent<string> onMenuFocused = new UnityEvent<string>();
    public static bool usingNavigation { get; private set; } = false;



    public static List<string> openMenus;
    private static UIManager instance;
    private static InputSystemUIInputModule inputModule;

    private bool gameOver = false;

    private void Awake()
    {
        onMenuOpened.AddListener(s => 
        {
            if (!openMenus.Contains(s)) 
            { 
                openMenus.Add(s);
            } 
        });
        onMenuClosed.AddListener(s => openMenus.Remove(s));
    }

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            openMenus = FindObjectsByType<Menu>(FindObjectsSortMode.None)
                .Where(m => m.isOpen)
                .Select(m => m.menuName)
                .ToList();



            inputModule = FindAnyObjectByType<InputSystemUIInputModule>();
            inputModule.move.action.performed += ControllerInputReceived;
            inputModule.leftClick.action.performed += MouseInputReceived;
            inputModule.cancel.action.performed += GoBack;
            DefeatManager.onPlayerLost?.AddListener(s => gameOver = true);
        }
    }

    private void GoBack(CallbackContext context)
    {
        if (openMenus.Count <= 1)
            return;

        onMenuClosed?.Invoke(openMenus.Last());
    }

    private static void ControllerInputReceived(CallbackContext context)
    {
        if (!usingNavigation && openMenus.Count > 0)
        {
            usingNavigation = true;
            onMenuFocused?.Invoke(openMenus[openMenus.Count - 1]);
        }
    }

    private static void MouseInputReceived(CallbackContext context)
    {
        if (usingNavigation && openMenus.Count > 0)
        {
            usingNavigation = false;
        }
    }

    private void Update()
    {
        if (!AllegianceManager.gameStarted)
            return;

        if(openMenus.Count > 0 || gameOver)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    public static bool IsMenuOpen(string menuName)
    {
        return openMenus.Contains(menuName);
    }
}
