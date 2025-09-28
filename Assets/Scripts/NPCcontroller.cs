using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class NPCcontroller : MonoBehaviour
{
    [HideInInspector]
    public string idleDialogueText;
    [HideInInspector]
    public string workDialogueText;
    [HideInInspector]
    public string[] playerChoices;
    [HideInInspector]
    public string[] npcReplies;
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
    private int SelectedChoiceIndex = 1;
    float interactionDistance = 1.5f;
    public KeyCode interact = KeyCode.F;
    public GameObject prompt;
    [HideInInspector]


    void Update()
    {
        if (isInteracting) return;
        float distance = Vector3.Distance(player.position, transform.position);
        if (distance <= interactionDistance)
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
        if (DialogueHandler.Instance != null)
        {
            DialogueHandler.Instance.ShowDialogue(gameObject.name, idleDialogueText);
        }
        yield return new WaitForSeconds(2f);
        if (DialogueHandler.Instance != null)
        {
            DialogueHandler.Instance.HideDialogue();
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

        if (DialogueHandler.Instance != null)
        {
            DialogueHandler.Instance.ShowDialogue(gameObject.name, workDialogueText);
        }
        Debug.Log("calling coroutine");
        yield return StartCoroutine(WaitForPlayerChoice());
        if (SelectedChoiceIndex >= 0 && SelectedChoiceIndex < npcReplies.Length)
        {
            string reply = npcReplies[SelectedChoiceIndex];

            if (DialogueHandler.Instance != null)
            {
                DialogueHandler.Instance.ShowDialogue(gameObject.name, reply);
            }
        }
        yield return new WaitForSeconds(3f);
        if (DialogueHandler.Instance != null)
        {
            DialogueHandler.Instance.HideDialogue();
        }
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

    private IEnumerator WaitForPlayerChoice()
    {
        SelectedChoiceIndex = 3;

        if (DialogueHandler.Instance != null)
        {
            DialogueHandler.Instance.ShowChoices(playerChoices, SetSelectedChoice);
            Debug.Log("trying to write choices");
        }
        yield return new WaitUntil(() => SelectedChoiceIndex != 3);
    }

    public void SetSelectedChoice(int choiceIndex)
    {
        SelectedChoiceIndex = choiceIndex;
    }
}
