using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public string loadMainMenu = "MainMenu";
    public void goToMainMenu()
    {
        GameManager.Instance.LoadSceneWithTransition(loadMainMenu); 
    }
}
