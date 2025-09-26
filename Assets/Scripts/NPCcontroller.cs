using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class NPCcontroller : MonoBehaviour
{
    public FirstPersonController playerController;
    public TurnScript turnScript;
    public NPCmanager npcManager;
    public Transform player;
    public Transform workStationTarget;
    private Animator animator;
    public bool hasMoved = false;
    public bool hasTeleported = false;
    public bool hasTalkedOnce = false;
    public bool isInteracting = false;
    public bool hasSat;
    float interactionDistance = 1f;
    public KeyCode interact = KeyCode.F;
    public GameObject prompt;
    [HideInInspector]
    public string assignedDialogueText;

    void Update()
    {
            float distance = Vector3.Distance(player.position, transform.position);
            if (distance <= interactionDistance && !isInteracting)
            {
                if (prompt != null)
                {
                    prompt.SetActive(true);
                }
                if (Input.GetKeyDown(interact))
                {
                    OnPlayerInteraction();
                }
            }
            else
            {
                prompt?.gameObject.SetActive(false);
            }

    }

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void TriggerStartTyping()
    {
        animator.SetTrigger("StartTyping");
    }

    public void TriggerStartSitting()
    {
        animator.SetTrigger("StartSitting");
    }

    public void TriggerStopSitting()
    {
        animator.SetTrigger("StopSitting");
    }

    public void OnPlayerInteraction()
    {
        if (hasTeleported)
        {
            StartCoroutine(InteractionSequence());
        }
        else if (!hasTeleported)
        {
            StartCoroutine(StandinInteractionSequence());
        }
        
    }

    private IEnumerator StandinInteractionSequence()
    {
        isInteracting = true;
        if (playerController != null)
        {
            playerController.enabled = false;
        }
        if (prompt != null)
        {
            prompt.SetActive(false);
        }
        if (DialogueHandler.Insatance != null)
        {
            DialogueHandler.Insatance.ShowDialogue(gameObject.name, assignedDialogueText);
        }
        yield return new WaitForSeconds(2f);
        if (DialogueHandler.Insatance != null)
        {
            DialogueHandler.Insatance.HideDialogue();
        }
        if (playerController != null)
        {
            playerController.enabled = true;
        }
        if (prompt != null)
        {
            prompt.SetActive(true);
        }
        isInteracting = false;
    }
    private IEnumerator InteractionSequence()
    {
        isInteracting = true;
        if (playerController != null)
        {
            playerController.enabled = false;
        }
        if (prompt != null)
        {
            prompt.SetActive(false);
        }
        turnScript.SaveOriginalRotationPosition();
        TriggerStartSitting();
        yield return StartCoroutine(turnScript.TurnToPlayer());
        if (DialogueHandler.Insatance != null)
        {
            DialogueHandler.Insatance.ShowDialogue(gameObject.name, assignedDialogueText);
        }
        Debug.Log("NPC is interacting with player");
        yield return new WaitForSeconds(5f);
        if (DialogueHandler.Insatance != null)
        {
            DialogueHandler.Insatance.HideDialogue();
        }
        Debug.Log("NPC has finished interacting with player");
        yield return StartCoroutine(turnScript.ReturnToOriginalRotation());
        TriggerStopSitting();
        if (playerController != null)
        {
            playerController.enabled = true;
        }
        if (prompt != null)
        {
            prompt.SetActive(true);
        }
        isInteracting = false;
    }
}
