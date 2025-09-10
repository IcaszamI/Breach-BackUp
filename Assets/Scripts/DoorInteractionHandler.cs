using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorInteractionHandler : MonoBehaviour
{
    [Header("Player Controller")]
    public FirstPersonController playerController;
    [Header("Player")]
    public Transform player;
    [Header("Door")]
    public Transform door;
    [Header("Others")]
    public GameObject prompt;
    public float interactionDistance = 1f;
    public KeyCode interact = KeyCode.F;

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        Debug.Log("Player's distance away the object is " + distance);

        if (distance <= interactionDistance)
        {
            if (prompt != null)
            {
                prompt.SetActive(true);
            }
            if (Input.GetKeyDown(interact))
            {
                SceneManager.LoadScene("Office");
            }
        }
        else
        {
            prompt?.gameObject.SetActive(false);
        }
    }
}
