using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSpawnController : MonoBehaviour
{
    public Transform player;
    public Transform DoorSpawn;

    void Awake()
{
    Debug.Log("DoorSpawnController.Awake() running.");

    if (GameManager.Instance != null && player != null)
    {
        Debug.Log("Player object found: " + player.gameObject.name);
        if (GameManager.Instance.AfterHours)
        {
            player.position = DoorSpawn.position;
            player.rotation = DoorSpawn.rotation;
            Debug.Log("Player position set to DoorSpawn at: " + player.position);
        }
    }
    else
    {
        if (player == null)
            Debug.LogError("DoorSpawnController: 'Player' reference is missing in the Inspector!");
        if (GameManager.Instance == null)
            Debug.LogError("DoorSpawnController: GameManager.Instance is null. Is it loaded?");
    }
}

}
