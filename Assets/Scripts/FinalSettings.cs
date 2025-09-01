using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StarterAssets;
using TMPro;

public class FinalSettings : MonoBehaviour
{
    [Header("Assign UI elements")]
    public Slider sensitivitySlider;
    public TMP_InputField sensitivityInput;
    public Slider volumeSlider;
    public TMP_InputField volumeInput;
    public FirstPersonController playerController;

    void Start()
    {
        float sensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 0.15f);
        sensitivitySlider.value = sensitivity;
        if (playerController != null) playerController.RotationSpeed = sensitivity * 100f;

        float volume = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
        volumeSlider.value = volume * 100f;
        AudioListener.volume = volume;

        sensitivitySlider.onValueChanged.AddListener(SaveSensitivity);
        volumeSlider.onValueChanged.AddListener(SaveVolume);

        if (sensitivityInput != null) sensitivityInput.text = sensitivitySlider.value.ToString("F2");
        if (volumeInput != null) volumeInput.text = volumeSlider.value.ToString("F0");

    }

    public void SaveSensitivity(float value)
    {
        if (playerController != null) playerController.RotationSpeed = value * 100f;
        PlayerPrefs.SetFloat("MouseSensitivity", value);
        PlayerPrefs.Save();
        if (sensitivityInput != null) sensitivityInput.text = value.ToString("F2");
    }

    public void SaveVolume(float value)
    {
        float normalizedVolume = value / 100f;
        AudioListener.volume = normalizedVolume;
        PlayerPrefs.SetFloat("MasterVolume", normalizedVolume);
        PlayerPrefs.Save();
        if (volumeInput != null) volumeInput.text = value.ToString("F0");

    }
}
