using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuLogic : MonoBehaviour
{
    public string loadOffice = "Office";
    public string loadSettings = "Settings";
    public string loadMenu = "MainMenu";
    public string loadHome = "Home";


    public void ContinueGame()
    {
        
    }
    public void startNewGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.currentDay = 1;
            GameManager.Instance.hasSeenIntroDialogue = false;
            GameManager.Instance.hasSeenCriteria = false;
            GameManager.Instance.hasSeenEmail = false;
            GameManager.Instance.processedEmailsToday.Clear();
            GameManager.Instance.mistakesMadeToday.Clear();
        }
        GameManager.Instance.LoadSceneWithTransition(loadHome);
    }
    public void settings()
    {
        SceneManager.LoadScene(loadSettings);
    }
    public void backToMenu()
    {
        Time.timeScale = 1;
        GameManager.Instance.LoadSceneWithTransition(loadMenu);

    }
    public void quitGame()
    {
        Application.Quit();
    }
}
