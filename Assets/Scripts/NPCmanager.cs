using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCmanager : MonoBehaviour
{
    public NPCcontroller[] npcs;
    public Transform[] idlingSPots;
    public Transform[] workstationSpots;
    public TurnScript turnScriptPrefab;
    void Start()
    {
        Shuffle(idlingSPots);
        
        for (int i = 0; i < npcs.Length; i++)
        {
            if (i < idlingSPots.Length)
            {
                npcs[i].transform.position = idlingSPots[i].position;
                npcs[i].transform.rotation = idlingSPots[i].rotation;

                if (!npcs[i].hasTeleported)
                {
                    if (idlingSPots[i].gameObject.name.Contains("SpawnbyWaterDispenser") && !npcs[i].hasTalkedOnce)
                    {
                        npcs[i].assignedDialogueText = "Man I'm thirsty";
                        npcs[i].hasTalkedOnce = true;
                    }
                    else if (idlingSPots[i].gameObject.name.Contains("SpawnbyFer") && !npcs[i].hasTalkedOnce)
                    {
                        npcs[i].assignedDialogueText = "Our employee of the month is.... a cat?";
                        npcs[i].hasTalkedOnce = true;
                    }
                    else if (idlingSPots[i].gameObject.name.Contains("SpawnbyFilingCabinet") && !npcs[i].hasTalkedOnce)
                    {
                        npcs[i].assignedDialogueText = "Why do we still use these?";
                        npcs[i].hasTalkedOnce = true;
                    }

                    if (i < workstationSpots.Length)
                    {
                        npcs[i].workStationTarget = workstationSpots[i];
                    }
                }
                

            }
        }
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
