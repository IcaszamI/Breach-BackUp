using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewText", menuName = "Phone System/Text Data")]
public class TextData : ScriptableObject
{
    public string textSender;
    public string textSubject;
    public string senderInitials;
    public Color photoColor;
    [TextArea(3, 10)]
    public string textdetails;
    [TextArea(1, 5)]
    public string response1;
    [TextArea(1, 5)]
    public string response2;
    
}
