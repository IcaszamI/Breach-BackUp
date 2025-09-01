using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    public ScreenCursor screenCursor;
    public CursorManager cursor;
    public FirstPersonController playerController;
    [Header("Pause UI")]
    public GameObject pauseMenuUI;

    [Header("Settings UI")]
    public GameObject SettingsUI;

    [Header("Blur")]
    public GameObject blurVolume;

    private bool isPaused = false;

    void Start()
    {
        pauseMenuUI?.SetActive(false);
        blurVolume?.SetActive(false);
        SettingsUI?.SetActive(false);
        Resume();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        isPaused = false;
        if (screenCursor != null)
        {
            screenCursor.enabled = false;
        }
        pauseMenuUI?.SetActive(false);
        blurVolume?.SetActive(false);
        SettingsUI?.SetActive(false);

        if (playerController != null)
            playerController.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        isPaused = true;

        pauseMenuUI?.SetActive(true);
        blurVolume?.SetActive(true);
        SettingsUI?.SetActive(false);
        if (screenCursor != null)
        {
            screenCursor.enabled = false;
        }

        if (playerController != null)
            playerController.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void openSettings()
    {
        pauseMenuUI?.SetActive(false);
        SettingsUI?.SetActive(true);

        if (playerController != null)
            playerController.enabled = false;

    }

    public void closeSettings()
    {
        SettingsUI?.SetActive(false);
        pauseMenuUI?.SetActive(true);

        if (playerController != null)
            playerController.enabled = false;

    }
}
