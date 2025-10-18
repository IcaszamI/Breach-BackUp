using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomeEmailManager : MonoBehaviour
{
    private const int totalEMailsPerDay = 8;
    private const int maxEmailsReceivedPerLoad = 3;
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
    public Button replyButton;
    public Button ignoreButton;
    [Header("Mistake UI")]
    public Transform mistakeButtonContainer;
    public GameObject mistakeButtonPrefab;
    public TextMeshProUGUI mistakeDetails;
    [Header("GameState")]
    public int maxTally = 3;
    private List<EmailData> activeEmails = new List<EmailData>();
    private List<EmailData> emailQueue = new List<EmailData>();
    private List<EmailData> mistakesMade = new List<EmailData>();

    private EmailData currentEmail;
    private GameObject currentEmailButton;
    public int mistakeTally = 0;
    private bool currentAttachmentHasBeenScanned = false;
    private Coroutine activeProgressBarCoroutine;


    [Header("Day 2 attachment UI")]
    public GameObject attachmentOptionsPanel;
    public GameObject progressBarPanel;
    public Slider progressBar;
    public TextMeshProUGUI progressText;
    public GameObject resultPanel;
    public TextMeshProUGUI resultText;
    public HelperManager helperManager;
    public AudioClip newEmailSound;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
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
        emailQueue.Clear();
        activeEmails.Clear();
        if (GameManager.Instance == null || !GameManager.Instance.AfterHours)
        {
            mistakesMade.Clear();
            mistakeTally = 0;
        }
        tallyCounter.text = mistakeTally.ToString();
        foreach (Transform child in emailButtonContainer)
        {
            Destroy(child.gameObject);
        }

        List<EmailData> emailCandidates = new List<EmailData>();
        EmailData firstEmail = null;
        List<EmailData> emailsReceivedEarlier = (GameManager.Instance != null && GameManager.Instance.emailsReceivedToday != null)? GameManager.Instance.emailsReceivedToday : new List<EmailData>();
        foreach (var email in allEmails)
        {
            if (email.dayAppears == day)
            {
                if (!emailsReceivedEarlier.Contains(email))
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
        }
        int totalEmailsTodaySoFar = emailsReceivedEarlier.Count;
        int remainingEmailsForToday = totalEMailsPerDay - totalEmailsTodaySoFar;
        int emailsToLoadThisSession = Mathf.Min(maxEmailsReceivedPerLoad, remainingEmailsForToday);
        Debug.Log($"Emails received earlier today: {totalEmailsTodaySoFar}. Loading {emailsToLoadThisSession} new emails this session.");

        for (int i = 0; i < emailCandidates.Count; i++)
        {
            EmailData temp = emailCandidates[i];
            int randomIndex = Random.Range(i, emailCandidates.Count);
            emailCandidates[i] = emailCandidates[randomIndex];
            emailCandidates[randomIndex] = temp;
        }

        for (int i = 0; i < Mathf.Min(emailsToLoadThisSession, emailCandidates.Count); i++)
        {
            EmailData emailToAdd = emailCandidates[i];
            emailQueue.Add(emailCandidates[i]);

            if (GameManager.Instance != null && GameManager.Instance.emailsReceivedToday != null)
            {
                GameManager.Instance.emailsReceivedToday.Add(emailToAdd);
            }
        }

        if (day == 1 && firstEmail != null && !emailsReceivedEarlier.Contains(firstEmail))
        {
            emailQueue.Insert(0, firstEmail);
            if (GameManager.Instance != null && GameManager.Instance.emailsReceivedToday != null)
            {
                GameManager.Instance.emailsReceivedToday.Add(firstEmail);
            }
        }

        int initialEmailCount = 1;
        for (int i = 0; i < initialEmailCount && emailQueue.Count > 0; i++)
        {
            AddNewEmail(emailQueue[0]);
            emailQueue.RemoveAt(0);
        }

        StartCoroutine(EmailDeliveryCoroutine());
    }

    private IEnumerator EmailDeliveryCoroutine()
    {
        while (emailQueue.Count > 0)
        {
            float delay = Random.Range(10f, 30f);
            yield return new WaitForSeconds(delay);
            AddNewEmail(emailQueue[0]);
            emailQueue.RemoveAt(0);
        }
        Debug.Log("All emails have been delivered");
    }

    private void AddNewEmail(EmailData email)
    {
        if (activeEmails.Count >= 1)
        {
            if (audioSource != null && newEmailSound != null)
            {
                audioSource.PlayOneShot(newEmailSound);
            }
        }

        if (email == null) return;
        activeEmails.Add(email);
        GameObject btnObj = Instantiate(emailButtonPrefab, emailButtonContainer);
        btnObj.transform.localScale = Vector3.one;
        btnObj.GetComponentInChildren<TextMeshProUGUI>().text = email.subject;
        btnObj.GetComponent<Button>().onClick.AddListener(() => ShowEmail(email, btnObj));
    }
    
    public void TryDeliverEmailOnSit()
    {
        if (emailQueue.Count > 0)
        {
            if (Random.value < 0.1f)
            {
                EmailData emailToDeliver = emailQueue[0];
                emailQueue.RemoveAt(0);
                AddNewEmail(emailToDeliver);
            }
            else
            {
                Debug.Log("Chance failed");
            }
        }
        else
        {
            Debug.Log("No more emails to deliver");
        }
    }
      
    public void ShowEmail(EmailData email, GameObject buttonObj)
    {
        
        currentEmail = email;
        currentEmailButton = buttonObj;
        if (email.hasAttachment)
        {
            replyButton?.gameObject.SetActive(false);
        }
        else
        {
            replyButton?.gameObject.SetActive(true);
        }
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
        if (GameManager.Instance != null)
        {
            int timeToAdvance = Random.Range(5, 10);
            GameManager.Instance.AdvanceTime(timeToAdvance);
        }
        clearContents();
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
        if (currentEmail.isMalicious)
        {
            Mistake(currentEmail.mistakeExplanation, currentEmail);
        }
        activeProgressBarCoroutine = StartCoroutine(ShowProgressBar("Downloading...", 1.5f, () =>
        {
            showResult("File Downloaded.", true);
        }));
    }

    public void OnReply()
    {
        if (fromText.text == "")
        {
            return;
        }
        else
        {
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
        
    }

    public void OnReport()
    {
        if (fromText.text == "")
        {
            return;
        }
        else
        {

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
        
    }

    void Mistake(string reason, EmailData email)
    {
        Debug.Log("Mistake Made: " + reason);
        if (helperManager != null)
        {
            helperManager.ShowRandomMistakeMessage();
        }
        mistakeTally++;
        tallyCounter.text = mistakeTally.ToString();
        if (!mistakesMade.Contains(email))
        {
            mistakesMade.Add(email);
        }
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
