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
        if (GameManager.Instance != null)
        {
            GameManager.Instance.currentDay = 1;
            GameManager.Instance.processedEmailsToday.Clear();
            GameManager.Instance.mistakesMadeToday.Clear();
        }
        SceneManager.LoadScene(loadOffice);
    }
    public void settings()
    {
        SceneManager.LoadScene(loadSettings);
    }
    public void backToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(loadMenu);

    }
    public void quitGame()
    {
        Application.Quit();
    }
}
