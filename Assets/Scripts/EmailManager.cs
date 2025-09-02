using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EmailManager : MonoBehaviour
{
    [Header("Email Data")]
    public List<EmailData> allEmails;
    [Header("UI reference")]
    public Transform emailButtonContainer;
    public GameObject emailButtonPrefab;
    public GameObject attachment;
    public TextMeshProUGUI fromText;
    public TextMeshProUGUI subjectText;
    public TextMeshProUGUI bodyText;
    public TextMeshProUGUI attachmentName;
    public TextMeshProUGUI tallyCounter;
    public GameObject emailPanel;
    [Header("Mistake UI")]
    public Transform mistakeButtonContainer;
    public GameObject mistakeButtonPrefab;
    public TextMeshProUGUI mistakeDetails;
    [Header("GameState")]
    public int maxTally = 3;
    private List<EmailData> todaysEmails = new();
    private List<EmailData> mistakesMade = new List<EmailData>();
    private List<EmailData> processedEmailsToday = new List<EmailData>();

    private EmailData currentEmail;
    private GameObject currentEmailButton;
    public int mistakeTally = 0;
    private int dailyEmails;

    void Start()
    {
        if (GameManager.Instance != null)
        {
            LoadEmailsForDay(GameManager.Instance.currentDay);
        }
        else
        {
            Debug.Log("GameManager not found, Starting day 1 by default");
            LoadEmailsForDay(1);
        }
    }

    public void LoadEmailsForDay(int day)
    {
        dailyEmails = day == 1 ? 4 : 5;
        todaysEmails.Clear();
        foreach (Transform child in emailButtonContainer)
        {
            Destroy(child.gameObject);
        }

        List<EmailData> emailCandidates = new();
        EmailData firstEmail = null;

        foreach (var email in allEmails)
        {
            if (email.dayAppears == day)
            {
                if (email.isFirstEmail && day == 1)
                {
                    firstEmail = email;
                }
                else
                {
                    emailCandidates.Add(email);
                }
            }
        }

        for (int i = 0; i < emailCandidates.Count; i++)
        {
            EmailData temp = emailCandidates[i];
            int randomIndex = Random.Range(i, emailCandidates.Count);
            emailCandidates[i] = emailCandidates[randomIndex];
            emailCandidates[randomIndex] = temp;
        }

        for (int i = 0; i < Mathf.Min(dailyEmails, emailCandidates.Count); i++)
        {
            todaysEmails.Add(emailCandidates[i]);
        }

        if (day == 1 && firstEmail != null)
        {
            todaysEmails.Insert(0, firstEmail);
        }

        foreach (var email in todaysEmails)
        {
            GameObject btnObj = Instantiate(emailButtonPrefab, emailButtonContainer);
            btnObj.GetComponentInChildren<TextMeshProUGUI>().text = email.subject;
            btnObj.GetComponent<Button>().onClick.AddListener(() => ShowEmail(email, btnObj));
        }
    }

    public void ShowEmail(EmailData email, GameObject buttonObj)
    {
        currentEmail = email;
        currentEmailButton = buttonObj;
        fromText.text = "From: " + email.senderEmail;
        subjectText.text = "Subject: " + email.subject;
        bodyText.text = email.body;
        emailPanel.SetActive(true);
        if (email.hasAttachment)
        {
            attachment.SetActive(true);
            attachmentName.text = email.attachmentName;
        }
        else
        {
            attachment.SetActive(false);
        }
    }

    public void OnReply()
    {
        processedEmailsToday.Add(currentEmail);
        if (!currentEmail.isFriendlyEmail)
        {
            Mistake("Shouldn't have replied", currentEmail);
        }

        Destroy(currentEmailButton);
        clearContents();
        CheckDayCompletion();
    }

    public void OnReport()
    {
        processedEmailsToday.Add(currentEmail);
        if (currentEmail.isFriendlyEmail)
        {
            Mistake("Shouldn't have Ignored", currentEmail);
        }

        Destroy(currentEmailButton);
        clearContents();
        CheckDayCompletion();

    }

    void Mistake(string reason, EmailData email)
    {
        mistakeTally++;
        tallyCounter.text = mistakeTally.ToString();
        mistakesMade.Add(email);

        if (mistakeTally >= maxTally)
        {
            mistakesMade.Clear();
            mistakeTally = 0;
            tallyCounter.text = mistakeTally.ToString();
            SceneManager.LoadScene("RepeatDayScene");
        }
    }

    void CheckDayCompletion()
    {
        StartCoroutine(CheckAfterFrame());
    }

    IEnumerator CheckAfterFrame()
    {
        yield return null;

        if (emailButtonContainer.childCount == 0)
        {
            Debug.Log("Day Complete, Transitioniung to report scene");
            if (GameManager.Instance != null)
            {
                GameManager.Instance.CompleteDay(processedEmailsToday, mistakesMade);
            }
        }
        
    }

    public void buildMistakePanel()
    {
        foreach (Transform child in mistakeButtonContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (EmailData mistake in mistakesMade)
        {
            Debug.Log("No. of mistakes in list: " + mistakesMade.Count);
            GameObject btnObj = Instantiate(mistakeButtonPrefab, mistakeButtonContainer);
            Button button = btnObj.GetComponent<Button>();
            if (button != null)
            {
                button.interactable = true;
            }
            btnObj.GetComponentInChildren<TextMeshProUGUI>().text = mistake.subject;
            btnObj.GetComponent<Button>().onClick.AddListener(() => displayMistakeDetails(mistake));
        }
    }

    public void displayMistakeDetails(EmailData mistakeToShow)
    {
        mistakeDetails.text = "";
        string details = $"<b><color=red>Mistake Explanation :</color></b>\n{mistakeToShow.mistakeExplanation}\n\n" +
                         $"<b><color=green> Correct Action :</color></b>\n{mistakeToShow.correctActionHint}\n\n" +
                         $"<b>Consequences:</b>\n{mistakeToShow.consequenceText}";

        mistakeDetails.text = details;
    }

    void clearContents()
    {
        fromText.text = "";
        subjectText.text = "";
        bodyText.text = "";
        attachment.SetActive(false);
    }
}
