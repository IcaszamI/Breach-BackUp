using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class SittingInteraction : MonoBehaviour
{
    [Header("player controller")]
    public FirstPersonController playerController;
    [Header("player")]
    public Transform player;
    [Header("sitting position")]
    public Transform sitPos;
    [Header("sit prompt")]
    public GameObject sitPromt;
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
    public float interactionDistance = 1f;
    public KeyCode sit = KeyCode.F;

    public bool isSiting = false;

    void Update()
    {

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= interactionDistance && !isSiting)
        {
            if (sitPromt != null)
            {
                sitPromt.SetActive(true);
            }
            if (Input.GetKeyDown(sit))
            {
                sitDown();
            }
        }

        else
        {
            sitPromt.SetActive(false);
        }

    }

    void sitDown()
    {
        isSiting = true;
        if (playerCam != null)
        {
            playerCam.SetActive(false);
            gameCam.SetActive(true);
        }

        if (screenUI != null)
        {
            screenUI.SetActive(true);
            CriteriaUI.SetActive(false);
            EmailUI.SetActive(false);
            power.SetActive(false);
        }

        if (playerController != null)
            playerController.enabled = false;

        if (screenCursor != null)
        {
            screenCursor.enabled = true;
            Debug.Log("tried to enabled");
        }
    }

    public void standUp()
    {
        isSiting = false;
        
        if (playerCam != null)
        {
            playerCam.SetActive(true);
            gameCam.SetActive(false);
        }

        if (screenUI != null)
        {
            screenUI.SetActive(false);
        }
        if (power != null)
        {
            power.SetActive(false);
        }

        if (playerController != null)
            playerController.enabled = true;

        if (screenCursor != null)
        {
            screenCursor.enabled = false;
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
