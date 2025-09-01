using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuLogic : MonoBehaviour
{
    public string loadOffice = "Office";
    public string loadSettings = "Settings";
    public string loadMenu = "MainMenu";

    public void startNewGame()
    {
        SceneManager.LoadScene(loadOffice);
    }
    public void settings()
    {
        SceneManager.LoadScene(loadSettings);
    }
    public void backToMenu()
    {
        SceneManager.LoadScene(loadMenu);

    }
    public void quitGame()
    {
        Application.Quit();
    }
}
