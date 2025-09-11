using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [Header("TransitionSettings")]
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 0.5f;
    private bool isTransitioning = false;

    public int currentHour { get; private set; }
    public int currentMinute { get; private set; }
    public static event Action<int, int> OnTimeChanged;
    private Coroutine dayTimerCoroutine;
    public int mistakeTally = 0;
    public int maxTally = 3;
    public int currentDay = 1;
    public List<EmailData> processedEmailsToday = new List<EmailData>();
    public List<EmailData> mistakesMadeToday = new List<EmailData>();
    public bool AfterHours = false;


    private void OnEnable() { SceneManager.sceneLoaded += OnSceneLoaded; }
    private void OnDisable() { SceneManager.sceneLoaded -= OnSceneLoaded; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (fadeCanvasGroup != null)
            {
                DontDestroyOnLoad(fadeCanvasGroup.transform.parent.gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void LoadSceneWithTransition(string sceneName)
    {
        if (!isTransitioning)
        {
            StartCoroutine(TransitionCoroutine(sceneName));
        }
    }

    private IEnumerator TransitionCoroutine(string sceneName)
    {
        isTransitioning = true;
        float timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            fadeCanvasGroup.alpha = timer / fadeDuration;
            yield return null;
        }
        fadeCanvasGroup.alpha = 1;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }
        asyncLoad.allowSceneActivation = true;
        yield return new WaitForSeconds(0.1f);
        timer = 0;
        while (timer < fadeDuration)
        { 
            timer += Time.unscaledDeltaTime;
            fadeCanvasGroup.alpha = 1f - (timer / fadeDuration);
            yield return null;
        }
        fadeCanvasGroup.alpha = 0;
        isTransitioning = false;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Office")
        {
            StartDay();
        }
        if (scene.name == "Home" && !AfterHours)
        {
            StartPreDay();
        }
        if (scene.name == "Home" && AfterHours)
        {
            StartPostDay();
        }
    }

    public void StartPreDay()
    {
        AfterHours = false;
        currentHour = 7;
        currentMinute = 0;

        if (dayTimerCoroutine != null)
        {
            StopCoroutine(dayTimerCoroutine);
        }
        dayTimerCoroutine = StartCoroutine(DayTimerCoroutine());
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

    public void StartPostDay()
    {
        currentHour = 17;
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
            LoadSceneWithTransition("RepeatDayScene");
        }
        if (mistakes.Count < 3)
        {
            LoadSceneWithTransition("NextDayScene");
        }


    }

    public void GoHomeForNextDay()
    {
        AfterHours = true;
        LoadSceneWithTransition("Home");
    }

    public void StartNextDay()
    {
        currentDay++;
        mistakeTally = 0;
        processedEmailsToday.Clear();
        mistakesMadeToday.Clear();
        if (currentDay > 4)
        {
            LoadSceneWithTransition("WinScene");
        }

        else
        {
            LoadSceneWithTransition("Office");
        }
    }

    public void AdvanceTime(int minutesToAdd)
    {
        currentMinute += minutesToAdd;
        while (currentMinute >= 60)
        {
            currentMinute -= 60;
            currentHour++;
        }
        OnTimeChanged?.Invoke(currentHour, currentMinute);
    }

    public void RepeatDay()
    {
        AfterHours = false;
        mistakeTally = 0;
        processedEmailsToday.Clear();
        mistakesMadeToday.Clear();
        LoadSceneWithTransition("Home");
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Home")
        {
            if (currentHour == 9)
            {
                StopTimer();
            }
        }
    }
}
