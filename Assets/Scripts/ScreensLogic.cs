using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

public class ScreensLogic : MonoBehaviour
{
    [Header("Screen")]
    public GameObject screenUI;
    [Header("Power")]
    public GameObject powerUI;
    [Header("Email")]
    public GameObject emailUI;
    [Header("Criteria")]
    public GameObject criteriaUI;
    [Header("Mistakes")]
    public GameObject mistakesUI;
    [Header("Icons")]
    public GameObject criteriaIcon;
    public GameObject powerIcon;
    public GameObject emailIcon;

    public void openEmail()
    {
        if (emailUI != null && !emailUI.activeInHierarchy)
        {
            emailUI.SetActive(true);
            criteriaIcon.SetActive(false);
            powerIcon.SetActive(false);
            emailIcon.SetActive(false);
        }
    }

    public void opencriteria()
    {
        if (criteriaUI != null && !criteriaUI.activeInHierarchy)
        {
            criteriaUI.SetActive(true);
            criteriaIcon.SetActive(false);
            powerIcon.SetActive(false);
            emailIcon.SetActive(false);
        }
    }

    public void openMistakes()
    {
        if (mistakesUI != null && !mistakesUI.activeInHierarchy)
        {
            mistakesUI.SetActive(true);
        }
        else
        {
            mistakesUI.SetActive(false); 
        }
    }
    public void powerOff()
    {
        if (powerUI != null && !powerUI.activeInHierarchy)
        {
            powerUI.SetActive(true);
        }
        else
        {
            powerUI.SetActive(false);
        }
    }

    public void emailExitButton()
    {
        if (emailUI != null && emailUI.activeInHierarchy)
        {
            emailUI.SetActive(false);
            criteriaIcon.SetActive(true);
            powerIcon.SetActive(true);
            emailIcon.SetActive(true);
        }
    }

    public void criteriaExitButton()
    {
        if (criteriaUI != null && criteriaUI.activeInHierarchy)
        {
            criteriaUI.SetActive(false);
            criteriaIcon.SetActive(true);
            powerIcon.SetActive(true);
            emailIcon.SetActive(true);
        }
    }

    public void mistakeExitButton()
    {
        if (mistakesUI != null && mistakesUI.activeInHierarchy)
        {
            mistakesUI.SetActive(false);
        }
    }
}
