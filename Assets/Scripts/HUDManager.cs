using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI sitQuestText;
    public TextMeshProUGUI emailQuestText;
    public TextMeshProUGUI criteriaQuestText;
    public TextMeshProUGUI HomeQuests;
    public GameObject helper;
    private int emailsProcessed = 0;
    private int totalEmailsToday = 0;
    public bool sitQuestCompleted;
    public bool emailQuestCompleted;
    public bool criteriaQuestCompleted;

    void OnEnable()
    {
        GameManager.OnTimeChanged += UpdateTimeText;
    }

    void OnDisable()
    {
        GameManager.OnTimeChanged -= UpdateTimeText;
    }
    void Start()
    {
        if (GameManager.Instance != null)
        {
            dayText.text = $"Day {GameManager.Instance.currentDay}";
            UpdateTimeText(GameManager.Instance.currentHour, GameManager.Instance.currentMinute);
        }

        if (SceneManager.GetActiveScene().name == "Office")
        {
            HomeQuests?.gameObject.SetActive(false);
            InitializeSitQuest();
            criteriaQuestText?.gameObject.SetActive(false);
            emailQuestText?.gameObject.SetActive(false);
        }

        if (SceneManager.GetActiveScene().name == "Home")
        {
            sitQuestText?.gameObject.SetActive(false);
            criteriaQuestText?.gameObject.SetActive(false);
            emailQuestText?.gameObject.SetActive(false);
            HomeQuests?.gameObject.SetActive(true);
            SetHomeQuestsText();
        }
        
    }

    public void SetHomeQuestsText()
    {
        if (HomeQuests != null && !GameManager.Instance.AfterHours)
        {
            HomeQuests.text = "- (Optional)Check personal emails.\n- Go to work.";
        }
        else
        {
            HomeQuests.text = "- OptionalCheck personal emails.\n- Go to bed";
        }
    }

    void UpdateTimeText(int hour, int minute)
    {
        string amPM = (hour < 12) ? "AM" : "PM";
        int displayHour = hour % 12;
        if (displayHour == 0)
        {
            displayHour = 12;
        }
        timeText.text = $"{displayHour:00}:{minute:00} {amPM}";
    }

    public void SetEmailQuestGoal(int totalEmails)
    {
        totalEmailsToday = totalEmails;
        emailsProcessed = 0;
        UpdateEmailQuestText();
    }

    public void UpdateEmailQuestProgress()
    {
        emailsProcessed++;
        UpdateEmailQuestText();
    }

    private void UpdateEmailQuestText()
    {
        if (emailQuestText != null)
        {
            emailQuestText.text = $"- Answer Emails ({emailsProcessed}/{totalEmailsToday}).";
            if (emailsProcessed >= 5)
            {
                emailQuestCompleted = true;
                emailQuestText.text = $"- <s>Answer Emails ({emailsProcessed}/{totalEmailsToday}).</s>";
                emailQuestText.color = Color.grey;
            }
        }
    }

    public void InitializeSitQuest()
    {
        if (sitQuestText != null)
        {
            sitQuestCompleted = false;
            sitQuestText.text = "- Sit on your chair";
            sitQuestText.color = Color.white;
        }
        if (criteriaQuestText != null)
        {
            criteriaQuestText.gameObject.SetActive(false);
        }
    }

    public void CompleteSitQuest()
    {
        if (sitQuestCompleted || sitQuestText == null) return;
        sitQuestCompleted = true;
        sitQuestText.text = "- <s>Sit on your chair.</s>";
        sitQuestText.color = Color.grey;

        if (criteriaQuestText != null)
        {
            criteriaQuestText.gameObject.SetActive(true);
            InitializeCriteriaQuest();
        }
    }

    public void InitializeCriteriaQuest()
    {
        if (criteriaQuestText != null)
        {
            criteriaQuestCompleted = false;
            criteriaQuestText.text = "- Check today's criteria.";
            criteriaQuestText.color = Color.white;
        }

        if (emailQuestText != null)
        {
            emailQuestText.gameObject.SetActive(false);
        }
    }

    public void CompleteCriteriaQuest()
    {
        if (criteriaQuestCompleted || criteriaQuestText == null) return;
        criteriaQuestCompleted = true;
        criteriaQuestText.text = "- <s>Check today's criteria.</s>";
        criteriaQuestText.color = Color.grey;

        if (emailQuestText != null)
        {
            emailQuestText.gameObject.SetActive(true);
            UpdateEmailQuestText();
        }
    }
}
