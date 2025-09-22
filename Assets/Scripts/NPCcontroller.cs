using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;

public class NPCcontroller : MonoBehaviour
{
    public FirstPersonController playerController;
    public Transform NPCtransform;
    public Transform player;
    public Transform workStationTarget;
    private Animator animator;
    public SittingInteraction sit;
    private bool hasMoved = false;
    float interactionDistance = 2f;
    public KeyCode interact = KeyCode.F;
    public GameObject prompt;


    void Update()
    {
        if (sit.hasSat && !hasMoved)
        {
            MoveToWorkStation();
            hasMoved = true;
        }
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= interactionDistance)
        {
            if (prompt != null)
            {
                prompt.SetActive(true);
            }
            if (Input.GetKeyDown(interact))
            {
               Debug.Log("spin NPC");
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
    private void MoveToWorkStation()
    {
        if (workStationTarget != null)
        {
            transform.position = workStationTarget.position;
            transform.rotation = workStationTarget.rotation;
            TriggerStartTyping();
        }
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
        TriggerStartSitting();
        Debug.Log("NPC is interacting with player");
        yield return new WaitForSeconds(5f);
        Debug.Log("NPC has finished interacting with player");
        TriggerStopSitting();
    }
}
