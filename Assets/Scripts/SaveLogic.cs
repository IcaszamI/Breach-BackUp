using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLogic : MonoBehaviour
{
    string loadMenu = "MainMenu";
    string loadOffice = "Office";


    public void SaveAndExit()
    {
        GameManager.Instance.LoadSceneWithTransition("MainMenu");
    }

    public void OnClickGoHome()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartNextDay();
        }
    }

    public void OnClickRepeatDay()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RepeatDay();
        }
    }
}
