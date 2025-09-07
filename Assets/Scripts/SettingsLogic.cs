using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using StarterAssets;
using Unity.VisualScripting;


public class SettingsLogic : MonoBehaviour
{
    public static event System.Action<float> OnSensitivityChanged;
    [Header("References")]
    public FirstPersonController playerController;
    [Header("Mouse Sensitivity")]
    public Slider sensitivitySlider;
    public TMP_InputField sensitivityInput;
    [Header("In-Game volume")]
    public Slider volumeSlider;
    public TMP_InputField volumeInput;

    private bool isUpdatingFromCode = false;

    void OnEnable()
    {
        LoadSettings();
        ApplySettings();
        SetupListeners();
    }

    void OnDisable()
    {
        sensitivitySlider.onValueChanged.RemoveAllListeners();
        volumeSlider.onValueChanged.RemoveAllListeners();
        sensitivityInput.onEndEdit.RemoveAllListeners();
        volumeInput.onEndEdit.RemoveAllListeners();
    }

    void LoadSettings()
    {
        float sensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 0.20f);
        sensitivitySlider.value = sensitivity;
        UpdateInputField(sensitivityInput, sensitivity);
        float volume = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
        volumeSlider.value = volume * 100f;
        UpdateInputField(volumeInput, volumeSlider.value);
    }
    void ApplySettings()
    {
        if (playerController != null)
        {
            playerController.RotationSpeed = sensitivitySlider.value * 100f;
        }

        AudioListener.volume = volumeSlider.value;
    }

    void SetupListeners()
    {
        OnDisable();
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
        OnSensitivityChanged.Invoke(value);

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
        float normalizedVolume = value / 100f;

        AudioListener.volume = normalizedVolume;
        PlayerPrefs.SetFloat("MasterVolume", normalizedVolume);
        PlayerPrefs.Save();
        UpdateInputField(volumeInput, value);
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
        if (inputField == sensitivityInput)
        {
            inputField.text = value.ToString("F2");
        }
        else if (inputField == volumeInput)
        {
            inputField.text = value.ToString("F0");
        }
    }
}
