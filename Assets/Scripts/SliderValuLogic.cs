using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderValuLogic : MonoBehaviour
{
[Header("Mouse Sensitivity")]
    public Slider sensitivitySlider;
    public TMP_InputField sensitivityInput;

    [Header("In-Game Volume")]
    public Slider volumeSlider;
    public TMP_InputField volumeInput;

    private bool isUpdating = false;

    void Start()
    {
        // Initialize values
        UpdateInput(sensitivitySlider, sensitivityInput);
        UpdateInput(volumeSlider, volumeInput);

        // Listeners
        sensitivitySlider.onValueChanged.AddListener((value) => OnSliderChanged(sensitivitySlider, sensitivityInput));
        volumeSlider.onValueChanged.AddListener((value) => OnSliderChanged(volumeSlider, volumeInput));

        sensitivityInput.onEndEdit.AddListener((input) => OnInputChanged(input, sensitivitySlider, sensitivityInput));
        volumeInput.onEndEdit.AddListener((input) => OnInputChanged(input, volumeSlider, volumeInput));
    }

    void UpdateInput(Slider slider, TMP_InputField input)
    {
        input.text = slider.value.ToString("F2");
    }

    void OnSliderChanged(Slider slider, TMP_InputField input)
    {
        if (isUpdating) return;
        isUpdating = true;

        UpdateInput(slider, input);

        if (slider == sensitivitySlider)
        {
            // Update your sensitivity logic
            // Example: MouseLook.sensitivity = slider.value;
        }
        else if (slider == volumeSlider)
        {
            AudioListener.volume = slider.value;
        }

        isUpdating = false;
    }

    void OnInputChanged(string inputText, Slider slider, TMP_InputField inputField)
    {
        if (isUpdating) return;
        if (float.TryParse(inputText, out float value))
        {
            value = Mathf.Clamp(value, slider.minValue, slider.maxValue);
            slider.value = value; // Will trigger OnSliderChanged
        }
        else
        {
            inputField.text = slider.value.ToString("F2");
        }
    }
}
