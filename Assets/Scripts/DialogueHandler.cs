using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueHandler : MonoBehaviour
{
    public static DialogueHandler Instance;
    public GameObject dialogueCanvas;
    public TextMeshProUGUI npcNameText;
    public TextMeshProUGUI dialogueText;
    public GameObject choicePanel;
    public Button[] choiceButtons;
    private System.Action<int> onChoiceMade;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        dialogueCanvas?.SetActive(false);
        choicePanel?.SetActive(false);

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            int choiceIndex = i;
            choiceButtons[i].onClick.AddListener(() => ChoiceSelected(choiceIndex));
        }
    }

    public void ShowDialogue(string npcName, string dialogue)
    {
        if (dialogueCanvas != null)
        {
            dialogueCanvas.SetActive(true);
            if (npcNameText != null)
            {
                npcNameText.text = npcName;
            }
            if (dialogueText != null)
            {
                dialogueText.text = dialogue;
            }
        }

        HideChoices();
    }

    public void HideDialogue()
    {
        if (dialogueCanvas != null)
        {
            dialogueCanvas.SetActive(false);
        }

        HideChoices();
    }

    public void ShowChoices(string[] choices, System.Action<int> callback)
    {
        Debug.Log("method called");
        onChoiceMade = callback;
        if (choicePanel != null)
        {
            choicePanel.SetActive(true);
        }
        Debug.Log("Panel opened");
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            Debug.Log("for loop called");
            if (i < choices.Length)
            {
                choiceButtons[i].gameObject.SetActive(true);
                Debug.Log("buttons active");
                choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = choices[i];
                Debug.Log("buttons interactivity added");
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void HideChoices()
    {
        if (choicePanel != null)
        {
            choicePanel.SetActive(false);
        }
    }

    public void ChoiceSelected(int index)
    {
        HideChoices();
        onChoiceMade?.Invoke(index);
    }
}
