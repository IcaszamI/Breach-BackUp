using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperMessagesScript : MonoBehaviour
{
    public HelperManager helperManager;
    void Start()
    {
        if (GameManager.Instance.currentDay == 1 && !GameManager.Instance.hasSeenIntroDialogue)
        {
            StartCoroutine(IntroDialogueSequence());
        }
    }

    private IEnumerator IntroDialogueSequence()
    {
        helperManager.showDialogue("Hi! My name is Fer, the company pet. Im here to help you get started.", 2f);
        yield return new WaitForSeconds(2.5f);
        helperManager.showDialogue("Have a look around the office and find your cubicle.", 2f);
        GameManager.Instance.hasSeenIntroDialogue = true;
    }
}
