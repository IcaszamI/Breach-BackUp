using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewGameSceneController : MonoBehaviour
{
    [Header("Text")]
    public TextMeshProUGUI introText;
    [TextArea(5, 15)]
    public string introMessage;
    public float typingSpeed = 0.3f;
    public float endDelay = 3.0f;
    void Start()
    {
        StartCoroutine(PlayIntroCoroutine());
    }

    private IEnumerator PlayIntroCoroutine()
    {
        introText.text = "";
        foreach (char letter in introMessage)
        {
            introText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(endDelay);
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadSceneWithTransition("Home");
        }
    }
}
