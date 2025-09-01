using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class MouseVolumeManager : MonoBehaviour
{
 [Header("Mouse Settings")]
    public Slider mouseSensitivitySlider;
    public FirstPersonController playerController;

    [Header("Audio Settings")]
    public Slider volumeSlider;

    private void Start()
    {
        float savedSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 0.1f);
        int savedVolume = PlayerPrefs.GetInt("GameVolume", 50);

        if (mouseSensitivitySlider != null)
            mouseSensitivitySlider.value = savedSensitivity;

        if (volumeSlider != null)
            volumeSlider.value = savedVolume;

        AudioListener.volume = savedVolume;

        if (playerController != null)
            playerController.RotationSpeed = savedSensitivity * 100f;

        if (mouseSensitivitySlider != null)
            mouseSensitivitySlider.onValueChanged.AddListener(UpdateMouseSensitivity);

        if (volumeSlider != null)
            volumeSlider.onValueChanged.AddListener(UpdateVolume);
    }

    public void UpdateMouseSensitivity(float value)
    {
        if (playerController != null)
        {
            playerController.RotationSpeed = value * 100f;
        }

        PlayerPrefs.SetFloat("MouseSensitivity", value);
    }

    public void UpdateVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("GameVolume", value);
    }
}
