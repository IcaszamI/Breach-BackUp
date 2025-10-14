using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;


public class CursorManager : MonoBehaviour
{
    public SittingInteraction sit;
    [Header("PauseUI")]
    public GameObject PauseCanvas;
    [Header("SettingsUI")]
    public GameObject SettingsCanvas;
    public static CursorManager instance;
    public NPCcontroller[] npcController;

    public bool isPaused = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateCursorState();
    }

    void Update()
    {
        UpdateCursorState();
        if (PauseCanvas.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        else if (SettingsCanvas.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        else if (sit.isSitting)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (isPaused == false)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        foreach (NPCcontroller npc in npcController)
        {
            if (npc.isInteracting && npc.hasTeleported)
            {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            }
        }


    }

    void UpdateCursorState()
    {
        string currentScene = SceneManager.GetActiveScene().name;


        if (currentScene == "MainMenu")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (currentScene == "Settings")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (currentScene == "NextDayScene")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void SetPaused(bool pause)
    {
        isPaused = pause;
        UpdateCursorState();
    }

} 
