using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Security;

public class EmailManager : MonoBehaviour
{
    [Header("Email Data")]
    public List<EmailData> allEmails;
    [Header("UI reference")]
    public Transform emailButtonContainer;
    public GameObject emailButtonPrefab;
    public GameObject attachmentButtonImage;
    public Button attachmentButton;
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

    [Header("Day 2 attachment UI")]
    public GameObject attachmentOptionsPanel;
    public GameObject progressBarPanel;
    public Slider progressBar;
    public TextMeshProUGUI progressText;
    public GameObject resultPanel;
    public TextMeshProUGUI resultText;


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

        if (attachmentButton != null)
        {
            attachmentButton.onClick.AddListener(OnClickAttachment);
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
        currentEmail.hasBeenScanned = false;
        attachmentOptionsPanel.SetActive(false);
        emailPanel.SetActive(true);
        if (email.hasAttachment)
        {
            attachmentButtonImage.SetActive(true);
            attachmentButton.GetComponentInChildren<TextMeshProUGUI>().text = email.attachmentName;
        }
        else
        {
            attachmentButtonImage.SetActive(false);
        }
    }

    public void OnClickAttachment()
    {
        attachmentOptionsPanel.SetActive(!attachmentOptionsPanel.activeSelf);
    }

    public void OnScanFile()
    {
        attachmentOptionsPanel.SetActive(false);
        EmailData emailToScan = currentEmail;
        StartCoroutine(ShowProgressBar("Scanning...", 2f, () =>
        {
            emailToScan.hasBeenScanned = true;
            if (emailToScan.isMalicious)
            {
                showResult("MALICIOUS FILE DETECTED!");
            }
            else
            {
                showResult("No Threats Found.");
            }
        }));
    }

    public void OnDownloadFile()
    {
        attachmentOptionsPanel.SetActive(false);
        if (!currentEmail.hasBeenScanned)
        {
            if (!currentEmail.isMalicious)
            {
                Mistake(currentEmail.mistakeExplanationUnscanned, currentEmail);
            }
            else
            {
                Mistake(currentEmail.mistakeExplanation, currentEmail);
            }
        }
        StartCoroutine(ShowProgressBar("Downloading...", 1.5f, () =>
        {
            showResult("File Downloaded.");
        }));
    }

    public void OnReply()
    {
        processedEmailsToday.Add(currentEmail);
        if (!currentEmail.isFriendlyEmail)
        {
            Mistake(currentEmail.mistakeExplanation, currentEmail);
        }
        else if (currentEmail.hasAttachment && !currentEmail.hasBeenScanned)
        {
            Mistake(currentEmail.mistakeExplanationUnscanned, currentEmail);
        }

        else
        {
            Debug.Log(" Correct Choice! ");
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
            Mistake(currentEmail.mistakeExplanation, currentEmail);
        }

        Destroy(currentEmailButton);
        clearContents();
        CheckDayCompletion();

    }

    void Mistake(string reason, EmailData email)
    {
        Debug.Log("Mistake Made: " + reason);
        mistakeTally++;
        tallyCounter.text = mistakeTally.ToString();
        if (!mistakesMade.Contains(email))
        {
            mistakesMade.Add(email);
        }

        if (mistakeTally >= maxTally)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.CompleteDay(mistakesMade, processedEmailsToday);
            }
            
        }
    }

    void CheckDayCompletion()
    {
        StartCoroutine(CheckAfterFrame());
    }

    IEnumerator ShowProgressBar(string text, float duration, System.Action onComplete)
    {
        Debug.Log("trying to activate progress bar");
        progressBarPanel.SetActive(true);
        Debug.Log("setting progress text");
        progressText.text = text;
        Debug.Log("starting progress loop");
        float timer = 0f;
        while (timer < duration)
        {
            if (progressBar != null)
            {
                progressBar.value = timer / duration;
            }
            timer += Time.deltaTime;
            yield return null;
        }
        Debug.Log("loop done");
        if (progressBar != null)
        {
            progressBar.value = 1f;
        }
        Debug.Log("closing progress bar");
        progressBarPanel.SetActive(false);
        onComplete?.Invoke();
    }
    void showResult(string text)
    {
        resultPanel.SetActive(true);
        resultText.text = text;
        StartCoroutine(CloseResultAfterDelay(2f));
    }

    IEnumerator CloseResultAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        resultPanel.SetActive(false);
    }

    IEnumerator CheckAfterFrame()
    {
        yield return null;

        if (emailButtonContainer.childCount == 0)
        {
            Debug.Log("Day Complete, Transitioniung to report scene");
            Debug.Log("Emailmanager is sending" + processedEmailsToday.Count + " processed and " + mistakesMade.Count + " mistakes");
            if (GameManager.Instance != null)
            {
                GameManager.Instance.CompleteDay(mistakesMade, processedEmailsToday);
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
        attachmentButton.gameObject.SetActive(false);
    }
}
