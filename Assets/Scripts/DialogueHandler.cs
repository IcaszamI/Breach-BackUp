using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StarterAssets;

public class DialogueHandler : MonoBehaviour
{
    public static DialogueHandler Insatance;
    public GameObject dialogueCanvas;
    public TextMeshProUGUI npcNameText;
    public TextMeshProUGUI dialogueText;

    private void Awake()
    {
        if (Insatance == null)
        {
            Insatance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (dialogueCanvas != null)
        {
            dialogueCanvas.SetActive(false);
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
    }

    public void HideDialogue()
    {
        if (dialogueCanvas != null)
        {
            dialogueCanvas.SetActive(false);
        }
    }

}
