using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int currentDay = 1;
    public List<EmailData> processedEmailsToday = new List<EmailData>();
    public List<EmailData> mistakesMadeToday = new List<EmailData>();

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

    public void CompleteDay(List<EmailData> mistakes, List<EmailData> processedEmails)
    {
        processedEmailsToday = new List<EmailData>(processedEmails);
        mistakesMadeToday = new List<EmailData>(mistakes);
        Debug.Log("Game manager has stored " + processedEmailsToday.Count + " processed emails and " + mistakesMadeToday.Count + " mistakes.");
        SceneManager.LoadScene("NextDayScene");
    }

    public void StartNextDay()
    {
        currentDay++;
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
        processedEmailsToday.Clear();
        mistakesMadeToday.Clear();
        SceneManager.LoadScene("Office");   
    }
}
