using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HelperManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    [Header("Dialogue Content")]
    public List<string> mistakeMessages;
    private Coroutine dialogueCoroutine;
    public GameObject helperMouthClosed;
    public GameObject helperMouthOpen;
    public AudioClip dialoguePopUpSound;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void showDialogue(string message, float duration)
    {
        if (dialogueCoroutine != null)
        {
            StopCoroutine(dialogueCoroutine);
        }
        dialogueCoroutine = StartCoroutine(DialogueCoroutine(message, duration));
    }

    public void ShowRandomMistakeMessage()
    {
        if (mistakeMessages == null || mistakeMessages.Count == 0)
        {
            Debug.LogError("No mistake messages available.");
            return;
        }
        int randomIndex = Random.Range(0, mistakeMessages.Count);
        showDialogue(mistakeMessages[randomIndex], 2f);
    }

    private IEnumerator DialogueCoroutine(string message, float duration)
    {

        dialogueText.text = message;
        helperMouthClosed.SetActive(false);
        helperMouthOpen.SetActive(true);
        if (audioSource != null && dialoguePopUpSound != null)
        {
            audioSource.PlayOneShot(dialoguePopUpSound);
        }
        dialoguePanel.SetActive(true);
        yield return new WaitForSeconds(duration);
        dialoguePanel.SetActive(false);
        helperMouthClosed.SetActive(true);
        helperMouthOpen.SetActive(false);
        dialogueCoroutine = null;
    }

}
