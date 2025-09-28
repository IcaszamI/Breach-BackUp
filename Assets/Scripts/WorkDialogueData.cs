using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
[CreateAssetMenu(fileName = "NewWorkDialogue", menuName = "Dialogues/Work Dialogue")]
public class WorkDialogueData : ScriptableObject
{
    [TextArea(3, 10)]
    public string dialogue;
    [TextArea(1, 5)]
    public string firstChoice;
    [TextArea(1, 5)]
    public string secondChoice;
    [TextArea(3, 10)]
    public string firstChoiceResponse;
    [TextArea(3, 10)]
    public string secondChoiceResponse;
    [Range(1, 4)]
    public int dayAppears;
}
