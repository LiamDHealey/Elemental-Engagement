using UnityEngine;

public class Menu : MonoBehaviour
{
    [Tooltip("The name of the menu this represents")]
    [SerializeField] private string _menuName;
    public string menuName => _menuName;

    public bool isOpen => transform.GetChild(0).gameObject.activeSelf;


    void Start()
    {
        UIManager.onMenuOpened.AddListener(OpenMenu);
        UIManager.onMenuClosed.AddListener(CloseMenu);        
    }

    private void OpenMenu(string menuName)
    {
        if(menuName == this._menuName)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void CloseMenu(string menuName) 
    { 
        if(menuName == this._menuName) 
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
