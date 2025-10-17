using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextButton : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI nameInitials;
    public TextMeshProUGUI textSubject;
    public Image photo;

    public void SetUp( TextData text)
    {
        nameText.text = text.textSender;
        nameInitials.text = text.senderInitials;
        photo.color = text.photoColor;
        textSubject.text = text.textSubject;
    }

}
