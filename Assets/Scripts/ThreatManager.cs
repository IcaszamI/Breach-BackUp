using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThreatManager : MonoBehaviour
{
    public Image threatIcon;
    public Image threatIconBorder;
    public TextMeshProUGUI threatName;
    public TextMeshProUGUI threatDescription;

    public List<ThreatData> day1Threats;
    public List<ThreatData> day2Threats;
    public List<ThreatData> day3Threats;
    public List<ThreatData> day4Threats;

    public List<Button> day1Buttons;
    public List<Button> day2Buttons;
    public List<Button> day3Buttons;
    public List<Button> day4Buttons;

    public GameObject noThreatsEncountered;
    public GameObject noThreatSelected;
    public GameObject threatselections;
    public void Start()
    {
        if (GameManager.Instance.AfterHours)
        {
            noThreatsEncountered.SetActive(false);
            noThreatSelected.SetActive(false);
            threatselections.SetActive(true);
            HideAllButtons();

            switch (GameManager.Instance.currentDay)
            {
                case 1:
                    SetupDay(day1Threats, day1Buttons);
                    break;
                case 2:
                    SetupDay(day2Threats, day2Buttons);
                    break;
                case 3:
                    SetupDay(day3Threats, day3Buttons);
                    break;
                case 4:
                    SetupDay(day4Threats, day4Buttons);
                    break;
                default:
                    break;
            }

            SetDefaultDisplay();
        }
        else
        {
            noThreatsEncountered.SetActive(true);
            noThreatSelected.SetActive(true);
            threatselections.SetActive(false);
        }
    }

    void SetupDay(List<ThreatData> dayThreats, List<Button> dayButtons)
    {
        for (int i = 0; i < dayButtons.Count; i++)
        {
            if (i < dayThreats.Count)
            {
                dayButtons[i].gameObject.SetActive(true);
                ThreatData currentThreat = dayThreats[i];
                dayButtons[i].onClick.RemoveAllListeners();
                dayButtons[i].onClick.AddListener(() => DisplayThreatInfo(currentThreat));
            }
        }
    }

    public void DisplayThreatInfo(ThreatData threat)
    {
        threatIcon.sprite = threat.icon;
        threatIcon.color = Color.white;
        threatName.text = threat.threatName;
        threatDescription.text = threat.threatDescription;
    }

    void HideAllButtons()
    {
        foreach (var button in day1Buttons) {button.gameObject.SetActive(false);}
        foreach (var button in day2Buttons) {button.gameObject.SetActive(false);}
        foreach (var button in day3Buttons) {button.gameObject.SetActive(false);}
        foreach (var button in day4Buttons) {button.gameObject.SetActive(false);}
    }

    void SetDefaultDisplay()
    {
        threatIcon.sprite = null;
        threatIconBorder.sprite = null;
        threatIcon.color = new Color(0, 0, 0, 0);
        threatIconBorder.color = new Color(0, 0, 0, 0);
        threatName.text = "";
        threatDescription.text = "";
    }
}

