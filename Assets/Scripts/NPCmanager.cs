using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public NPCcontroller[] npcs;
    public Transform[] idlingSPots;
    public Transform[] workstationSpots;
    void Start()
    {
        Shuffle(idlingSPots);

        for (int i = 0; i < npcs.Length; i++)
        {
            if (i < idlingSPots.Length)
            {
                npcs[i].transform.position = idlingSPots[i].position;
                npcs[i].transform.rotation = idlingSPots[i].rotation;

                if (i < workstationSpots.Length)
                {
                    npcs[i].workStationTarget = workstationSpots[i];
                }
            
            }
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
