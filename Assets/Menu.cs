using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [Tooltip("The name of the menu this represents")]
    [SerializeField] private string name;

    // Start is called before the first frame update
    void Start()
    {
        UiInputHandler.onMenuOpened.AddListener(OpenMenu);
        UiInputHandler.onMenuClosed.AddListener(CloseMenu);
        
    }

    private void OpenMenu(string menuName)
    {
        if(menuName == name)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void CloseMenu(string menuName) 
    { 
        if(menuName == name) 
        { 
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
