using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;

public class UIManager : MonoBehaviour
{
    public static bool isUIOpen => openMenus.Count > 0;
    public static UnityEvent<string> onMenuOpened = new UnityEvent<string>();
    public static UnityEvent<string> onMenuClosed = new UnityEvent<string>();



    private static HashSet<string> openMenus;
    private static UIManager instance;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            openMenus = FindObjectsByType<Menu>(FindObjectsSortMode.None)
                .Where(m => m.isOpen)
                .Select(m => m.menuName)
                .ToHashSet();
            onMenuOpened.AddListener(s => openMenus.Add(s));
            onMenuClosed.AddListener(s => openMenus.Remove(s));
        }
    }

    private void Update()
    {
        if(openMenus.Count > 0)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }


}
