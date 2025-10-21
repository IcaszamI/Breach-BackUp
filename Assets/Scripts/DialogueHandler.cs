using TMPro;
using UnityEngine;
using UnityEngine.UI;
using StarterAssets;
using System.Collections;

public class DialogueHandler : MonoBehaviour
{
    public static DialogueHandler Instance { get; private set; }
    public GameObject dialogueCanvas;
    public TextMeshProUGUI npcNameText;
    public TextMeshProUGUI dialogueText;
    public GameObject choicePanel;
    public Button[] choiceButtons;
    private StarterAssetsInputs playerInputs;
    private FirstPersonController playerController;
    private System.Action<int> onChoiceMade;
    private bool isDialogueActive = false;

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

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("PlayerRoot");
        if (player != null)
        {
            playerInputs = player.GetComponent<StarterAssetsInputs>();
            playerController = player.GetComponent<FirstPersonController>();

            if (playerController == null || playerInputs == null)
            {
                Debug.LogError("player controller and/or inputs not found.");
            }
        }
        else
        {
            Debug.LogError("no gameobject named PlayerRoot");
        }

        StartCoroutine(ForceLockOnInit());
    }

    private IEnumerator ForceLockOnInit()
    {
        yield return null;

        if (!isDialogueActive)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void Update()
    {
        if (isDialogueActive)
        {
            if (Cursor.lockState != CursorLockMode.None || !Cursor.visible)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
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

    public void EnterDialogueMode()
    {
        isDialogueActive = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (playerController != null)
        {
            playerController.enabled = false;
        }
        if (playerInputs != null)
        {
            playerInputs.cursorInputForLook = false;
        }
    }

    public void ExitDialogueMode()
    {
        isDialogueActive = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerController != null)
        {
            playerController.enabled = true;
        }
        if (playerInputs != null)
        {
            playerInputs.cursorInputForLook = true;
        }
    }

    public void EnterStandingDialogueMode()
    {
        if (playerController != null)
        {
            playerController.enabled = false;
        }
        if (playerInputs != null)
        {
            playerInputs.cursorInputForLook = false;
        }
    }

    public void ExitStandingDialogueMode()
    {
        if (playerController != null)
        {
            playerController.enabled = true;
        }
        if (playerInputs != null)
        {
            playerInputs.cursorInputForLook = true;
        }
    }
}
