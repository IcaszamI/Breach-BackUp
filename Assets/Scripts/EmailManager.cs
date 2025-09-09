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
    public HUDManager hudManager;
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
    public Button replyButton;
    public Button ignoreButton;
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
    private bool currentAttachmentHasBeenScanned = false;
    private Coroutine activeProgressBarCoroutine;


    [Header("Day 2 attachment UI")]
    public GameObject attachmentOptionsPanel;
    public GameObject progressBarPanel;
    public Slider progressBar;
    public TextMeshProUGUI progressText;
    public GameObject resultPanel;
    public TextMeshProUGUI resultText;



    public List<EmailData> GetProcessedEmails()
    {
        return processedEmailsToday;
    }

    public List<EmailData> GetMistakes()
    {
        return mistakesMade;
    }
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

        if (hudManager != null)
        {
            hudManager.SetEmailQuestGoal(todaysEmails.Count);
        }
    }

    public void ShowEmail(EmailData email, GameObject buttonObj)
    {
        currentEmail = email;
        currentEmailButton = buttonObj;
        fromText.text = "From: " + email.senderEmail;
        subjectText.text = "Subject: " + email.subject;
        bodyText.text = email.body;
        currentAttachmentHasBeenScanned = false;
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
    private void FinalizeEmailAction()
    {
        if (currentEmailButton != null)
        {
            Destroy(currentEmailButton);
        }
        if (hudManager != null)
        {
            hudManager.UpdateEmailQuestProgress();
        }
        clearContents();
        CheckDayCompletion();
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
            currentAttachmentHasBeenScanned = true;
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
        processedEmailsToday.Add(currentEmail);
        if (!currentAttachmentHasBeenScanned)
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
        activeProgressBarCoroutine = StartCoroutine(ShowProgressBar("Downloading...", 1.5f, () =>
        {
            showResult("File Downloaded.", true);
        }));
    }

    public void OnReply()
    {
        processedEmailsToday.Add(currentEmail);
        if (!currentEmail.isFriendlyEmail)
        {
            Debug.Log("mistakeExplanation was triggered");
            Mistake(currentEmail.mistakeExplanation, currentEmail);
        }
        else if (currentEmail.hasAttachment && !currentAttachmentHasBeenScanned)
        {
            Debug.Log("mistakeExplanationUnscanned was triggered");
            Mistake(currentEmail.mistakeExplanationUnscanned, currentEmail);
        }

        else
        {
            Debug.Log(" Correct Choice! ");
        }
        FinalizeEmailAction();
    }

    public void OnReport()
    {
        processedEmailsToday.Add(currentEmail);

        if (currentEmail.isFriendlyEmail)
        {
            Mistake(currentEmail.mistakeExplanation, currentEmail);
        }
        else if (!currentEmail.isFriendlyEmail && currentEmail.hasAttachment && !currentAttachmentHasBeenScanned)
        {
            Mistake(currentEmail.mistakeExplanationUnscanned, currentEmail);
        }
        else
        {
            Debug.Log("correct choice");
        }
        FinalizeEmailAction();
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
                GameManager.Instance.CompleteDay(processedEmailsToday, mistakesMade);
            }

        }
    }

    void CheckDayCompletion()
    {
        StartCoroutine(CheckAfterFrame());
    }

    IEnumerator ShowProgressBar(string text, float duration, System.Action onComplete)
    {
        if (ignoreButton != null) ignoreButton.interactable = false;
        if (replyButton != null) replyButton.interactable = false;
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
        if (ignoreButton != null) ignoreButton.interactable = true;
        if (replyButton != null) replyButton.interactable = true;
    }
    void showResult(string text, bool finalizeAfter = false)
    {
        resultPanel.SetActive(true);
        resultText.text = text;
        activeProgressBarCoroutine = StartCoroutine(CloseResultAfterDelay(0.5f,finalizeAfter));
    }

    IEnumerator CloseResultAfterDelay(float delay, bool finalizeAfter)
    {
        yield return new WaitForSeconds(delay);

        if (finalizeAfter)
        {
            FinalizeEmailAction();
        }
        resultPanel.SetActive(false);
        activeProgressBarCoroutine = null;
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
                GameManager.Instance.StopTimer();
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
        attachmentButton.gameObject.SetActive(false);
    }
    public void CloseAttachmentOptionPanel()
    {
        if (attachmentOptionsPanel.activeInHierarchy)
        {
            attachmentOptionsPanel.SetActive(false);
        }
    }
}
