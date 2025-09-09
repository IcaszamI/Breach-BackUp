using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int currentHour { get; private set; }
    public int currentMinute { get; private set; }
    public static event Action<int, int> OnTimeChanged;
    private Coroutine dayTimerCoroutine;
    public int mistakeTally = 0;
    public int maxTally = 3;
    public int currentDay = 1;
    public List<EmailData> processedEmailsToday = new List<EmailData>();
    public List<EmailData> mistakesMadeToday = new List<EmailData>();


    private void OnEnable(){ SceneManager.sceneLoaded += OnSceneLoaded; }
    private void OnDisable(){ SceneManager.sceneLoaded -= OnSceneLoaded;}
    void Awake()
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

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Office")
        {
            StartDay();
        }
    }

    public void StartDay()
    {
        currentHour = 9;
        currentMinute = 0;

        if (dayTimerCoroutine != null)
        {
            StopCoroutine(dayTimerCoroutine);
        }
        dayTimerCoroutine = StartCoroutine(DayTimerCoroutine());
    }

    public void StopTimer()
    {
        if (dayTimerCoroutine != null)
        {
            StopCoroutine(dayTimerCoroutine);
            dayTimerCoroutine = null;
        }
    }

    IEnumerator DayTimerCoroutine()
    {
        Debug.Log("Day Timer Started");
        while (true)
        {
            yield return new WaitForSeconds(2f);
            currentMinute++;

            if (currentMinute >= 60)
            {
                currentMinute = 0;
                currentHour++;
            }

            OnTimeChanged.Invoke(currentHour, currentMinute);

            if (currentHour >= 17)
            {
                Debug.Log("Time's up day failed");
                EmailManager emailManager = FindObjectOfType<EmailManager>();
                if (emailManager != null)
                {
                    CompleteDay(emailManager.GetMistakes(), emailManager.GetProcessedEmails());
                }
                yield break;
            }

        }
    }


    public void CompleteDay(List<EmailData> processedEmails, List<EmailData> mistakes)
    {
        StopTimer();
        processedEmailsToday = new List<EmailData>(processedEmails);
        mistakesMadeToday = new List<EmailData>(mistakes);
        Debug.Log("Game manager has stored " + processedEmailsToday.Count + " processed emails and " + mistakesMadeToday.Count + " mistakes.");
        if (mistakes.Count == 3)
        {
            SceneManager.LoadScene("RepeatDayScene");
        }
        else
        {
            SceneManager.LoadScene("NextDayScene");
        }

    }

    public void StartNextDay()
    {
        currentDay++;
        mistakeTally = 0;
        processedEmailsToday.Clear();
        mistakesMadeToday.Clear();
        if (currentDay > 4)
        {
            Debug.Log("tandaan mo lagyan congrats scene");
        }

        else
        {
            SceneManager.LoadScene("Office");
        }
    }

    public void RepeatDay()
    {
        mistakeTally = 0;
        processedEmailsToday.Clear();
        mistakesMadeToday.Clear();
        SceneManager.LoadScene("Office");   
    }
}
