using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlarmClockLogic : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    void OnEnable()
    {
        GameManager.OnTimeChanged += UpdateTimeText;
    }

    void OnDisable()
    {
        GameManager.OnTimeChanged -= UpdateTimeText;
    }

    void Start()
    {
        if (GameManager.Instance != null)
        {
            UpdateTimeText(GameManager.Instance.currentHour, GameManager.Instance.currentMinute);
        }
    }

    void UpdateTimeText(int hour, int minute)
    {
        string amPM = (hour < 12) ? "AM" : "PM";
        int displayHour = hour % 12;
        if (displayHour == 0)
        {
            displayHour = 12;
        }
        timeText.text = $"{displayHour:00}:{minute:00} {amPM}";
    }

}
