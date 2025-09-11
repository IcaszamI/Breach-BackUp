using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSpawnController : MonoBehaviour
{
    public Transform player;
    public Transform DoorSpawn;

    void Start()
    {
        if (GameManager.Instance != null && player != null)
        {
            if (GameManager.Instance.AfterHours)
            {
                player.position = DoorSpawn.position;
                player.rotation = DoorSpawn.rotation;
            }
        }
        
    }

}
