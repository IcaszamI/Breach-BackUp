using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuCursor : MonoBehaviour
{

    void Update()
    {
        Scene currenScene = SceneManager.GetActiveScene();
        string scene = currenScene.name;

        if (scene == "MainMenu")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (scene == "Settings")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (scene == "NextDayScene")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (scene == "RepeatDayScene")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
    }
}
