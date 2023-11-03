using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayGame : MonoBehaviour
{
    public void playGame()
    {
        SceneManager.LoadSceneAsync("Content/Scenes/MainScene");
    }

    public void quitGame()
    {
        Application.Quit();
    }
}