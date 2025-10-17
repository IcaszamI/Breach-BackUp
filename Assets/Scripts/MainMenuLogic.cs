using UnityEngine;

public class MainMenuLogic : MonoBehaviour
{
    public string loadOffice = "Office";
    public string loadSettings = "Settings";
    public string loadMenu = "MainMenu";
    public string loadHome = "Home";
    public string loadIntro = "NewGameScene";


    public void PlayGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.currentDay = 1;
            GameManager.Instance.hasSeenHomeIntro = false;
            GameManager.Instance.hasSeenOfficeIntro = false;
            GameManager.Instance.hasSeenCriteria = false;
            GameManager.Instance.firstTimeOpeningEmailInDay2 = false;
            GameManager.Instance.firstTimeOpeningCriteriaInDay3 = false;
            GameManager.Instance.hasSeenEmail = false;
            GameManager.Instance.processedEmailsToday.Clear();
            GameManager.Instance.mistakesMadeToday.Clear();
        }
        GameManager.Instance.LoadSceneWithTransition(loadIntro);
    }
    public void settings()
    {
        GameManager.Instance.LoadSceneWithTransition(loadSettings);
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
