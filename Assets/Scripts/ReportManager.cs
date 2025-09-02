using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ReportManager : MonoBehaviour
{
    [Header("UI references")]
    public Transform reportButtonContainer;
    public TextMeshProUGUI reportDetailsText;

    [Header("Prefabs & Colors")]
    public GameObject reportButtonPrefab;
    public Color correctColor = new Color(0.7f, 1.0f, 0.7f, 1.0f);
    public Color mistakeColor = new Color(1.0f, 0.7f, 0.7f, 1.0f);

    void Start()
    {
        PopulateReport();
    }

    void PopulateReport()
    {
        foreach (Transform child in reportButtonContainer)
        {
            Destroy(child.gameObject);
        }

        if (GameManager.Instance == null || GameManager.Instance.processedEmailsToday.Count == 0)
        {
            reportDetailsText.text = "No report data found for the previous data.";
            return;
        }

        reportDetailsText.text = "Select an email to preview.";

        foreach (var processedEmail in GameManager.Instance.processedEmailsToday)
        {
            GameObject btnObj = Instantiate(reportButtonPrefab, reportButtonContainer);
            btnObj.transform.localScale = Vector3.one;
            btnObj.GetComponentInChildren<TextMeshProUGUI>().text = processedEmail.subject;
            Image buttonImage = btnObj.GetComponent<Image>();

            if (GameManager.Instance.mistakesMadeToday.Contains(processedEmail))
            {
                if (buttonImage != null) buttonImage.color = mistakeColor;
            }
            else
            {
                if (buttonImage != null) buttonImage.color = correctColor;
            }
            btnObj.GetComponent<Button>().onClick.AddListener(() => DisplayReportDetails(processedEmail));
        }
    }
    void DisplayReportDetails(EmailData emailData)
    {
        string details = $"<b>Email Subject: </b>\n{emailData.subject}\n\n" +
        $"<b><color=red> Possible Consequence: </color></b>\n{emailData.consequenceText}";
        reportDetailsText.text = details;
    }

    public void onClickNextDay()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartNextDay();
        }
    }

    public void onClickExit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
