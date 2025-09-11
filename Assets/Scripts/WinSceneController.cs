using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinSceneController : MonoBehaviour
{
    [Header("UI references")]
    public TextMeshProUGUI headerText;
    public TextMeshProUGUI bodyText;
    public TextMeshProUGUI footerText;
    [Header("Text Settings")]
    public string headerMessage;
    [TextArea(5, 15)]
    public string bodyMessage;
    public string footerMessage;
    [Header("Timing Settings")]
    public float typingSpeed = 0.3f;
    public float endDelay = 3.0f;
    void Start()
    {
        StartCoroutine(PlayWinSequence());
    }

    private IEnumerator PlayWinSequence()
    {
        headerText.text = "";
        bodyText.text = "";
        footerText.text = "";
        foreach (char letter in headerMessage)
        {
            headerText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        foreach (char letter in bodyMessage)
        {
            bodyText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        foreach (char letter in footerMessage)
        {
            footerText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitForSeconds(endDelay);
        if (GameManager.Instance != null)
        {
            GameManager.Instance.currentDay = 1;
            GameManager.Instance.LoadSceneWithTransition("MainMenu");
        }
    }
}
