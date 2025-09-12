using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UI;

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
    [Header("BlackListed Apps")]
    public GameObject blackListedAppsUI;
    public HUDManager hudManager;
    [Header("Mistakes")]
    public GameObject mistakesUI;
    [Header("Icons")]
    public GameObject criteriaIcon;
    public GameObject powerIcon;
    public GameObject emailIcon;
    public GameObject blackListedAppsIcon;
    public HelperManager helperManager;


    void Update()
    {
        if (hudManager != null)
        {
            if (!hudManager.criteriaQuestCompleted)
            {
                emailIcon.GetComponent<Button>().interactable = false;
            }
            else
            {
                emailIcon.GetComponent<Button>().interactable = true;
            }
        }

        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.currentDay >= 3)
            {
                blackListedAppsIcon.SetActive(true);
            }
            else
            {
                blackListedAppsIcon.SetActive(false);
            }
        }
    }
    public void openEmail()
    {
        if (emailUI != null && !emailUI.activeInHierarchy)
        {
            emailUI.SetActive(true);
            criteriaIcon.SetActive(false);
            powerIcon.SetActive(false);
            emailIcon.SetActive(false);

            if (!GameManager.Instance.hasSeenEmail)
            {
                helperManager.showDialogue("This is the email App. Youll get emails throughout the day. Some might arrive later!", 2f);
                GameManager.Instance.hasSeenEmail = true;
            }
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
            hudManager.CompleteCriteriaQuest();

            if (!GameManager.Instance.hasSeenCriteria)
            {
                helperManager.showDialogue("This is the Criteria App. It shows the rules you need to follow for the day.", 2f);
                GameManager.Instance.hasSeenCriteria = true;
            }
        }
    }

    public void openBlackListedApps()
    {
        if (blackListedAppsUI != null && !blackListedAppsUI.activeInHierarchy)
        {
            blackListedAppsUI.SetActive(true);
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

    public void blackListedAppsExitButton()
    {
        if (blackListedAppsUI != null && blackListedAppsUI.activeInHierarchy)
        { 
            blackListedAppsUI.SetActive(false);
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
