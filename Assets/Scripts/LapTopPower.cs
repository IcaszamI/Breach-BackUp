using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapTopPower : MonoBehaviour
{
    public SittingInteraction sit;
    public GameObject screenOff;

    // Update is called once per frame
    void Update()
    {
        if (!sit.isSiting)
        {
            screenOff.SetActive(true);
        }
        else
        {
            screenOff.SetActive(false);
        }
        
    }
}
