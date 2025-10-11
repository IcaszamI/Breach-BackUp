using System.Collections;
using TMPro;
using UnityEngine;

public class RandomNotificationTrigger : MonoBehaviour
{
    [Header("Probability Settings")]
    [Range(0.01f, 100f)]
    public float chancePerSecond = 5f;

    [Header("UI references")]
    public GameObject phoneUIPanel;
    public TextMeshProUGUI notificationText;

    [Header("Notofication Content")]
    public string[] possibleNotifications = new string[]
    {
        "Reminder: Update your 2FA security key.",
        "Alert: Unusual login attempt detected on your work account.",
        "Warning: 3 pending unread emails from your manager.",
        "System: VPN connection status is currently unstable.",
        "Critical Patch: Apply immediately for security fix."
    };

    private bool isPhoneActive = false;
    private Coroutine timingCoroutine;

    void Start()
    {
        if (phoneUIPanel != null)
        {
            phoneUIPanel.SetActive(false);
        }
        timingCoroutine = StartCoroutine(NotificationCheckLoop());
    }

    private IEnumerator NotificationCheckLoop()
    {
        while (true)
        {
            float roll = Random.Range(0f, 100f);
            if (roll < chancePerSecond)
            {
                TriggerNotification();
            }
            yield return new WaitForSeconds(1.5f);
        }
    }

    public void TriggerNotification()
    {
        if (phoneUIPanel == null || notificationText == null || possibleNotifications.Length == 0)
        {
            Debug.LogError("Phone UI or notification text not found");
            return;
        }

        isPhoneActive = true;
        string randomMessage = possibleNotifications[Random.Range(0, possibleNotifications.Length)];
        notificationText.text = randomMessage;
        phoneUIPanel.SetActive(true);

        StartCoroutine(HideNotificationsAfterDelay(3f));
    }

    private IEnumerator HideNotificationsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        phoneUIPanel.SetActive(false);
        isPhoneActive = false;

    }
}
