using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewEmail", menuName = "Email System/Email Data")]
public class EmailData : ScriptableObject
{
    public string senderEmail;
    public string subject;
    [TextArea(3, 10)]
    public string body;
    public string attachmentName;
    public bool hasAttachment;
    public bool isMalicious;
    public bool isBlacklistedApp;
    public bool isCorrectDomain;
    public bool isFriendlyEmail;
    public bool isFirstEmail;
    [Range(1, 4)]
    public int dayAppears;
    [TextArea(1, 5)]
     public string mistakeExplanation; 
    [TextArea(1, 5)]
     public string correctActionHint;  
    [TextArea(1, 5)]
     public string consequenceText;
}
