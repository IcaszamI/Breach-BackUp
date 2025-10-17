using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HelperMessagesScript : MonoBehaviour
{
    public HelperManager helperManager;
    private const string HomeSceneName = "Home";
    private const string OfficeSceneName = "Office";

    void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        
        switch (currentScene)
        {
            case HomeSceneName:
                StartHomeSequence();
                break;
            case OfficeSceneName:
                StartOfficeSequence();
                break;
            default:
                break;
        }
    }

    private void StartHomeSequence()
    {
        if (GameManager.Instance.currentDay == 1 && !GameManager.Instance.hasSeenHomeIntro && !GameManager.Instance.AfterHours)
        {
            StartCoroutine(HomeIntroDialogueSequence());
        }
        else
        {
            if (GameManager.Instance.AfterHours && GameManager.Instance.currentDay == 1)
            {
                StartCoroutine(FirstAfterHoursSequence());
            }
        }
    }

    private void StartOfficeSequence()
    {
        if (GameManager.Instance.currentDay == 1 && !GameManager.Instance.hasSeenOfficeIntro)
        {
            StartCoroutine(IntroDialogueSequence());
        }
    }

    private IEnumerator IntroDialogueSequence()
    {
        helperManager.showDialogue("Hello again my name is Fer, the company pet. Im here to help you get started. and to be your guide throughout your time in Breach Networks", 5f);
        yield return new WaitForSeconds(5f);
        helperManager.showDialogue("Have a look around the office and find your cubicle.", 5f);
        GameManager.Instance.hasSeenOfficeIntro = true;
    }

    private IEnumerator HomeIntroDialogueSequence()
    {
        helperManager.showDialogue("Hi I'll introduce myself later for now you should look around your home", 5f);
        yield return new WaitForSeconds(5f);
        helperManager.showDialogue("You can either go to the office immediately or answer some personal emails at you laptop", 5f);
        GameManager.Instance.hasSeenHomeIntro = true;
    }

    private IEnumerator FirstAfterHoursSequence()
    {
        helperManager.showDialogue("It's been a long day right? you can choose to sleep immediately and start your next day or check your laptop", 5f);
        yield return new WaitForSeconds(5f);
        helperManager.showDialogue("there you'll see the threats you have faced up until now and you can also answer more personal emails if you'd like", 5f);
    }

    public IEnumerator FirstTimeOpeningEmailInDay2Sequence()
    {
        helperManager.showDialogue("To scan the file start by left clicking the file icon below the email's Body text then click scan.", 5f);
        yield return new WaitForSeconds(5f);
        helperManager.showDialogue("After scanning you'll see if the file is safe to download or if you should ignore it.", 5f);

    }


    public IEnumerator FirstTimeOpeningCriteriaInDay3Sequence()
    {
        helperManager.showDialogue("You can see which apps are blacklisted on the  BlkLstdApps.txt file on the desktop", 5f);
        yield return new WaitForSeconds(5f);
    }

    public IEnumerator FirstTimeStandinUp()
    {
        helperManager.showDialogue("You should walk around and see if any of your coworkers need help while waiting to receive more emails.", 5f);
        yield return new WaitForSeconds(5f);
    }
}
