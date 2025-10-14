using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class NPCmanager : MonoBehaviour
{
    public NPCcontroller[] npcs;
    public Transform[] idlingSPots;
    public Transform[] workstationSpots;
    public List<WorkDialogueData> allDialogue;
    public List<WorkDialogueData> allHelpDialogue;
    public SittingInteraction sit;
    private bool playerIsSitting;

    [Header("Immediate Spawn Interaction")]
    [Range(0f, 100f)]
    public float SpawnChance = 10f;
    public Transform spawnSpot;
    void Start()
    {
        Shuffle(idlingSPots);
        if (GameManager.Instance == null) return;
        int currentDay = GetCurrentDay();
        List<WorkDialogueData> dailyDialogue = allDialogue.Where(dialogue => dialogue.dayAppears == currentDay).ToList();
        List<WorkDialogueData> dailyHelpDialogue = allHelpDialogue.Where(dialogue => dialogue.dayAppears == currentDay).ToList();

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
                AssignHelpDialogue(npcs[i], dailyHelpDialogue);
            }
        }
    }

    public void TrySpawnRandomInteraction()
    {
        Debug.Log("trying to spawn interaction");
        playerIsSitting = sit.isSitting;

        if (!playerIsSitting) return;
        if (npcs.Any(n => n.isInteracting))
        {
            return;
        }
        Debug.Log($"spawn chance is currently {SpawnChance}");
        float roll = Random.Range(0f, 100f);
        if (roll < SpawnChance)
        {
            Debug.Log($"chance hit. Your roll was {roll}");
            SpawnNPC();
        }
        else
        {
            Debug.Log($"chance failed. Your roll was {roll}");
        }
    }
    
    private void SpawnNPC()
    {
        Debug.Log("spawning NPC");
        if (spawnSpot == null) return;
        NPCcontroller[] availableNPCs = npcs.Where(n => !n.isInteracting).ToArray();
        if (availableNPCs.Length == 0)
        {
            Debug.Log("No available NPCs to spawn");
            return;
        }
        int randomInder = Random.Range(0, availableNPCs.Length);
        NPCcontroller chosenNPC = availableNPCs[randomInder];
        chosenNPC.originalReturnPosition = chosenNPC.transform.position;
        chosenNPC.originalReturnRotation = chosenNPC.transform.rotation;
        chosenNPC.wasTyping = chosenNPC.hasTeleported;
        chosenNPC.transform.position = spawnSpot.position;
        chosenNPC.transform.rotation = spawnSpot.rotation;
        chosenNPC.StartImmediateInteraction();
        if (PlayerLookController.Instance != null)
        {
            PlayerLookController.Instance.StartCoroutine(PlayerLookController.Instance.TurnToFace(chosenNPC.transform.position));
        }
        else
        {
            Debug.LogError("PlayerLookController not found");
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

    public void AssignHelpDialogue(NPCcontroller npc, List<WorkDialogueData> dialogues)
    {
        if (dialogues.Count > 0)
        {
            Debug.Log("assigning help dialogue");
            int dialogueIndex = Random.Range(0, dialogues.Count);
            WorkDialogueData dialogue = dialogues[dialogueIndex];
            npc.helpDialoguetext = dialogue.dialogue;
            npc.helpChoices = new string[] { dialogue.firstChoice, dialogue.secondChoice };
            npc.helpReply = new string[] { dialogue.firstChoiceResponse, dialogue.secondChoiceResponse };
        }
        else
        {
            Debug.LogWarning($"No Help Dialogue found for current day!");
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
