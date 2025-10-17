using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PhoneManager : MonoBehaviour
{
    private const float checkIntervals = 5f;
    private const float textProbability = 100f;
    private const int maxTextPerDay = 4;
    public List<TextData> allTexts;
    public HUDManager hudManager;
    private List<TextData> receivedTextsToday = new List<TextData>();
    private TextData currentText;
    private Coroutine textCheckCoroutine;
    private AudioSource audioSource;
    public AudioClip newTextSound;
    public GameObject messagesUI;
    public GameObject textView;
    public Transform textButtonContainer;
    public GameObject textButtonPrefab;
    public TextMeshProUGUI senderName;
    public TextMeshProUGUI textDetails;
    public Button response1Button;
    public Button response2Button;
    public TextMeshProUGUI response1Text;
    public TextMeshProUGUI response2Text;

    void Awake()
    {
       audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        ShowList();
        LoadConversations();
        StartDailyCheck();

    }

    public void StartDailyCheck()
    {
        if (textCheckCoroutine != null)
        {
            StopCoroutine(textCheckCoroutine);
        }
        textCheckCoroutine = StartCoroutine(TextCheckCoroutine());
    }



    private IEnumerator TextCheckCoroutine()
    {
        while (receivedTextsToday.Count < maxTextPerDay)
        {
            yield return new WaitForSeconds(checkIntervals);
            float randomIndex = Random.Range(0f, 100f);
            if (randomIndex < textProbability)
            {
                GenerateNewText();
            }
        }

        Debug.Log("daily text limit reached");
        textCheckCoroutine = null;
    }

    private void GenerateNewText()
    {
        var availableTexts = allTexts.Except(receivedTextsToday).ToList();
        if (availableTexts.Count == 0)
        {
            Debug.LogWarning("No more texts left to send");
            return;
        }
        TextData newText = availableTexts[Random.Range(0, availableTexts.Count)];
        DeliverText(newText);
    }

    private void DeliverText(TextData text)
    {
        receivedTextsToday.Add(text);
        LoadConversations();
        if (audioSource != null && newTextSound != null)
        {
            audioSource.PlayOneShot(newTextSound);
        }
        if (hudManager != null)
        {
            hudManager.DisplayPhonePrompt();
        }
        if (receivedTextsToday.Count >= maxTextPerDay && textCheckCoroutine != null)
        {
            StopCoroutine(textCheckCoroutine);
            textCheckCoroutine = null;
        }
    }

    void LoadConversations()
    {
        var displayList = receivedTextsToday;
        foreach (Transform child in textButtonContainer)
        {
            Destroy(child.gameObject);
        }
        foreach (TextData textData in displayList)
        {
            Debug.Log("Populating buttons");
            GameObject btnObj = Instantiate(textButtonPrefab, textButtonContainer);
            TextButton button = btnObj.GetComponent<TextButton>();
            if (button != null)
            {
                button.SetUp(textData);
                btnObj.GetComponent<Button>().onClick.AddListener(() => OpenChat(textData));
            }
        }
    }

    public void OpenChat(TextData textData)
    {
        currentText = textData;

        senderName.text = textData.textSender;
        textDetails.text = textData.textdetails;
        response1Text.text = textData.response1;
        response2Text.text = textData.response2;
        response1Button.onClick.RemoveAllListeners();
        response2Button.onClick.RemoveAllListeners();
        response1Button.onClick.AddListener(() => HandleResponse(1));
        response2Button.onClick.AddListener(() => HandleResponse(2));
        ShowChat();

    }

    void HandleResponse(int response)
    {
        GameManager.Instance.AdvanceTime(5);

        CloseChat();
    }
    
    public void ShowList()
    {
        textView.SetActive(false);
    }

    public void ShowChat()
    {
        textView.SetActive(true);
    }

    public void CloseChat()
    {
        ShowList();
    }

    public void ResetForNewDay()
    {
        receivedTextsToday.Clear();
        LoadConversations();
        StartDailyCheck();
    }
    
    public void OpenMessages()
    {
        messagesUI?.SetActive(true);
    }
    
    public void OnClickHomeButton()
    {
        messagesUI?.SetActive(false);
    }
        
}

    

