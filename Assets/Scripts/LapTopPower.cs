using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapTopPower : MonoBehaviour
{
    public SittingInteraction sit;
    public GameObject screenOff;
    public GameObject powerOffPanel;

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

    public void OnClickPowerOffPanel()
    {
        if (!powerOffPanel.activeInHierarchy)
        {
            powerOffPanel.SetActive(true);
        }
        else
        {
            powerOffPanel?.SetActive(false);
        }
    }

    public void closePowerOffPanel()
    {
        powerOffPanel?.SetActive(false);
    }
        

}
