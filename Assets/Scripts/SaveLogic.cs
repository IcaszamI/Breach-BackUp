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
        SceneManager.LoadScene(loadMenu);
    }

    public void OnClickNextDay()
    {
        if (GameManager.Instance != null)
        {
            
            GameManager.Instance.StartNextDay();
        }
    }

    public void OnClickRepeatDay()
    {
        GameManager.Instance.RepeatDay();
    }
}
