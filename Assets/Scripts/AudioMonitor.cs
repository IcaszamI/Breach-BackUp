using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMonitor : MonoBehaviour
{
    public static AudioMonitor Instance;
    private float lastKnownVolume;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        lastKnownVolume = AudioListener.volume;
    }

    void Update()
    {
        if (AudioListener.volume != lastKnownVolume)
        {
            Debug.LogWarning($"AUDIO VOLUME CHANGED! New value: {AudioListener.volume}. Culprit is in stack trace below.", this.gameObject);
            Debug.Break(); // This will PAUSE the editor
            lastKnownVolume = AudioListener.volume;
        }
    }

    public void ReportLegitimateVolumeChange(float newVolume)
    {
        lastKnownVolume = newVolume;
    }
}
