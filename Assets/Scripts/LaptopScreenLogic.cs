using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaptopScreenLogic : MonoBehaviour
{
    public GameObject emailIcon;
    public GameObject threatsIcon;
    public GameObject emailUI;
    public GameObject threatsUI;
    public GameObject mistakeIcon;
    public GameObject mistakeUI;

    public void OpenEmail()
    {
        if (emailUI != null && !emailUI.activeInHierarchy)
        {
            emailUI.SetActive(true);
        }
    }

    public void OpenThreats()
    {
        if (threatsUI != null && !threatsUI.activeInHierarchy)
        {
            threatsUI.SetActive(true);
        }
    }

    public void OpenMistakes()
    {
        if (mistakeUI != null && !mistakeUI.activeInHierarchy)
        {
            mistakeUI.SetActive(true);
        }
    }

    public void closeEmail()
    {
        if (emailUI != null && emailUI.activeInHierarchy)
        {
            emailUI.SetActive(false);
        }
    }
    public void closeThreats()
    {
        if (threatsUI != null && threatsUI.activeInHierarchy)
        {
            threatsUI.SetActive(false);
        }
    }
    public void closeMistakes()
    {
        if (mistakeUI != null && mistakeUI.activeInHierarchy)
        {
            mistakeUI.SetActive(false);
        }
    }
}
