using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets; // Required to access FirstPersonController if you're using Starter Assets

public class GroundDebug : MonoBehaviour
{
    FirstPersonController controller;

    void Start()
    {
        controller = GetComponent<FirstPersonController>();
    }

    void Update()
    {
    }
}

