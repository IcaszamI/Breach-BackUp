using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NPCmanager : MonoBehaviour
{
    public NPCcontroller[] npcs;
    public Transform[] idlingSPots;
    public Transform[] workstationSpots;
    public List<WorkDialogueData> allDialogue;
    void Start()
    {
        Shuffle(idlingSPots);
        if (GameManager.Instance == null) return;
        int currentDay = GetCurrentDay();
        List<WorkDialogueData> dailyDialogue = allDialogue.Where(dialogue => dialogue.dayAppears == currentDay).ToList();

        
        for (int i = 0; i < npcs.Length; i++)
        {
            if (i < idlingSPots.Length)
            {
                npcs[i].transform.position = idlingSPots[i].position;
                npcs[i].transform.rotation = idlingSPots[i].rotation;
                npcs[i].hasTeleported = false;

                npcs[i].idleDialogueText = GetIdleDialogue(idlingSPots[i].name);
                npcs[i].hasTalkedOnce = true;

                AssignWorkDialogue(npcs[i], dailyDialogue);
            }
        }
    }

    private int GetCurrentDay()
    {

        return GameManager.Instance.currentDay;

        
    }

    public void MoveNPCToWorkstations()
    {
        for (int i = 0; i < npcs.Length; i++)
        {
            if (npcs[i].workStationTarget != null)
            {
                npcs[i].transform.position = npcs[i].workStationTarget.position;
                npcs[i].transform.rotation = npcs[i].workStationTarget.rotation;
                npcs[i].hasTeleported = true;
                npcs[i].TriggerStartTyping();
            }
        }
        foreach (NPCcontroller npc in npcs)
        {
            npc.hasMoved = true;
        }
    }

    private string GetIdleDialogue(string spotName)
    {
        if (spotName.Contains("SpawnbyWaterDispenser"))
        {
            return "Man I'm thirsty";
        }
        else if (spotName.Contains("SpawnbyFer"))
        {
            return "Our employee of the month is.... a cat?";
        }
        else if (spotName.Contains("SpawnbyFilingCabinet"))
        {
            return "Why do we still use these?";
        }
        else
        {
            return "Just getting ready for the day. Feel free to stop by my desk later!";
        }
    }

    private void AssignWorkDialogue(NPCcontroller npc, List<WorkDialogueData> dialogues)
    {
        if (dialogues.Count > 0)
        {
            Debug.Log("assigning work dialogue");
            int dialogueIndex = Random.Range(0, dialogues.Count);
            WorkDialogueData dialogue = dialogues[dialogueIndex];
            npc.workDialogueText = dialogue.dialogue;
            npc.playerChoices = new string[] { dialogue.firstChoice, dialogue.secondChoice };
            npc.npcReplies = new string[] { dialogue.firstChoiceResponse, dialogue.secondChoiceResponse };
        }
        else
        {
            Debug.LogWarning($"No Work Dialogue found for current day!");
        }
    }

    private void Shuffle(Transform[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Transform temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }
}
