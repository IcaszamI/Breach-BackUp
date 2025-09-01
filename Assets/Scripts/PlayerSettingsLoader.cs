using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class PlayerSettingsLoader : MonoBehaviour
{
    public FirstPersonController playerController;
    // Start is called before the first frame update
    void Start()
    {
        float savedSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 0.1f);

        if (playerController != null)
        {
            playerController.RotationSpeed = savedSensitivity * 100f;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
