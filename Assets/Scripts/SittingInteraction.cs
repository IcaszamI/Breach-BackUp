using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SittingInteraction : MonoBehaviour
{
    public  NPCmanager npc;
    public HUDManager hudManager;
    [Header("player controller")]
    public FirstPersonController playerController;
    [Header("player")]
    public Transform player;
    [Header("sitting position")]
    public Transform sitPos;
    [Header("sit prompt")]
    public GameObject prompt;
    [Header("game camera")]
    public GameObject gameCam;
    [Header("player camera")]
    public GameObject playerCam;
    [Header("Screen")]
    public GameObject screenUI;
    [Header("EmailUI")]
    public GameObject EmailUI;
    [Header("Criteria")]
    public GameObject CriteriaUI;
    [Header("Power")]
    public GameObject power;
    [Header("Screen Cursor Script")]
    public ScreenCursor screenCursor;
    public EmailManager emailManager;
    public NPCcontroller[] npcs;
    public float interactionDistance = 1f;
    public KeyCode sit = KeyCode.F;
    public bool isSitting = false;

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= interactionDistance && !isSitting)
        {
            if (prompt != null)
            {
                prompt.SetActive(true);
            }
            if (Input.GetKeyDown(sit))
            {
                sitDown();
            }
        }

        else
        {
            prompt?.SetActive(false);
        }

    }

    void sitDown()
    {
        if (playerController != null)
        {
            playerController.gameObject.SetActive(false);
        }
        npc?.MoveNPCToWorkstations();
        foreach (NPCcontroller npc in npcs)
        {
            if (npc != null)
            {
                npc.hasSat = true;
            }
        }
        isSitting = true;
        if (playerCam != null)
        {
            playerCam.SetActive(false);
            gameCam.SetActive(true);
        }

        if (screenUI != null)
        {
            if (SceneManager.GetActiveScene().name == "Office")
            {
                screenUI?.SetActive(true);
            }
            CriteriaUI?.SetActive(false);
            EmailUI?.SetActive(false);
            power?.SetActive(false);
        }

        if (playerController != null) playerController.enabled = false;
        screenCursor.enabled = true;
        Debug.Log("tried to enabled");
        hudManager.CompleteSitQuest();
        emailManager.TryDeliverEmailOnSit();
    }

    public void standUp()
    {
        isSitting = false;

        if (playerController != null)
        {
            playerController.gameObject.SetActive(true);
        }
        if (playerCam != null)
        {
            playerCam?.SetActive(true);
            gameCam?.SetActive(false);
        }

        if (screenUI != null)
        {
            if (SceneManager.GetActiveScene().name == "Office")
            {
                screenUI?.SetActive(false);
            }
            
        }
        if (power != null)
        {
            power?.SetActive(false);
        }

        if (playerController != null) playerController.enabled = true;
        screenCursor.enabled = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
