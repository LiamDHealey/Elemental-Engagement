using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [Tooltip("Path to the correct scene that this button loads")]
    [SerializeField] string sceneToLoad;
    public void LoadNewScene(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
