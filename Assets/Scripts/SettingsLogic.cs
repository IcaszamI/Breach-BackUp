using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using StarterAssets;
using Unity.VisualScripting;


public class SettingsLogic : MonoBehaviour
{
    [Header("References")]
    public FirstPersonController playerController;
    [Header("Mouse Sensitivity")]
    public Slider sensitivitySlider;
    public TMP_InputField sensitivityInput;
    [Header("In-Game volume")]
    public Slider volumeSlider;
    public TMP_InputField volumeInput;

    private bool isUpdatingFromCode = false;

    void Start()
    {
        LoadSettings();
        ApplySettings();
        SetupListeners();
    }

    void LoadSettings()
    {
        float sensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 0.20f);
        sensitivitySlider.value = sensitivity;
        UpdateInputField(sensitivityInput, sensitivity);

        float volume = PlayerPrefs.GetFloat("Volume", 50.0f);
        volumeSlider.value = volume;
        UpdateInputField(volumeInput, volume);


    }

    void ApplySettings()
    {
        if (playerController != null)
        {
            playerController.RotationSpeed = sensitivitySlider.value * 100f;
        }

        AudioListener.volume = volumeSlider.value;
        if (AudioMonitor.Instance != null) AudioMonitor.Instance.ReportLegitimateVolumeChange(volumeSlider.value);
    }

    void SetupListeners()
    {
        sensitivitySlider.onValueChanged.AddListener(OnSensitivitySliderChanged);
        volumeSlider.onValueChanged.AddListener(OnVolumeSliderChanged);

        sensitivityInput.onEndEdit.AddListener(OnSensitivityInputChanged);
        volumeInput.onEndEdit.AddListener(OnVolumeInputChanged);
    }

    private void OnSensitivitySliderChanged(float value)
    {
        if (isUpdatingFromCode) return;

        if (playerController)
        {
            playerController.RotationSpeed = value * 100f;
        }

        PlayerPrefs.SetFloat("MouseSensitivity", value);
        PlayerPrefs.Save();
        UpdateInputField(sensitivityInput, value);

    }

    private void OnSensitivityInputChanged(string text)
    {
        if (float.TryParse(text, out float value))
        {
            value = Mathf.Clamp(value, sensitivitySlider.minValue, sensitivitySlider.maxValue);
            isUpdatingFromCode = true;
            sensitivitySlider.value = value;
            isUpdatingFromCode = false;
        }
    }

    private void OnVolumeSliderChanged(float value)
    {
        if (isUpdatingFromCode) return;

        AudioListener.volume = value;
        PlayerPrefs.SetFloat("GameVolume", value);
        PlayerPrefs.Save();
        UpdateInputField(volumeInput, value);
        if (AudioMonitor.Instance != null) AudioMonitor.Instance.ReportLegitimateVolumeChange(value);
    }

    private void OnVolumeInputChanged(string text)
    {
        if (float.TryParse(text, out float value))
        {
            value = Mathf.Clamp(value, volumeSlider.minValue, volumeSlider.maxValue);
            isUpdatingFromCode = true;
            volumeSlider.value = value;
            isUpdatingFromCode = false;
        }
    }

    private void UpdateInputField(TMP_InputField inputField, float value)
    {
        inputField.text = value.ToString("F2");
    }
}
