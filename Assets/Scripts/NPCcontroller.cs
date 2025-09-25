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
    public Transform player;
    public Transform workStationTarget;
    private Animator animator;
    public GameObject NPCdialoguebox;
    public GameObject NPCdialogue;
    public GameObject NpcNamebox;
    public TextMeshProUGUI dialogue;
    public TextMeshProUGUI NPCname;
    public GameObject choice1;
    public GameObject choice2;
    public GameObject choice3;
    public bool hasMoved = false;
    public bool hasSat;
    float interactionDistance = 1f;
    public KeyCode interact = KeyCode.F;
    public GameObject prompt;


    void Update()
    {
        if (hasMoved)
        {
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
        StartCoroutine(InteractionSequence());
    }

    private IEnumerator InteractionSequence()
    {
        turnScript.SaveOriginalRotationPosition();
        TriggerStartSitting();
        yield return StartCoroutine(turnScript.TurnToPlayer());
        Debug.Log("NPC is interacting with player");
        yield return new WaitForSeconds(5f);
        Debug.Log("NPC has finished interacting with player");
        yield return StartCoroutine(turnScript.ReturnToOriginalRotation());
        TriggerStopSitting();
    }
}
