using System.Collections;
using StarterAssets;
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

    [HideInInspector]
    public string helpDialoguetext;
    [HideInInspector]
    public string[] helpChoices;
    [HideInInspector]
    public string[] helpReply;
    [HideInInspector]
    public Vector3 originalReturnPosition;
    public Quaternion originalReturnRotation;
    [HideInInspector]
    public bool wasTyping = false;

    public FirstPersonController playerController;
    public CameraTurn cameraTurn;
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
    private int SelectedChoiceIndex = -1;
    private bool isImmediateInteraction = false;
    float interactionDistance = 1.5f;
    public KeyCode interact = KeyCode.F;
    public GameObject prompt;
    public enum DialogueType { Idle, Work, Help }
    public DialogueType dialogueType;
    private AudioSource audioSource;
    public AudioClip Hmm;
    public AudioClip Hey;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

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

    public void TriggerStartIdling()
    {
        animator.SetTrigger("StartIdling");
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

    public void StartImmediateInteraction()
    {
        isImmediateInteraction = true;
        StartCoroutine(ImmediateInteractionSequence());
    }

    private IEnumerator ImmediateInteractionSequence()
    {
        TriggerStartIdling();
        isImmediateInteraction = true;
        isInteracting = true;
        if (prompt != null)
        {
            prompt.SetActive(false);
        }
        if (audioSource != null && Hey != null)
        {
            audioSource.PlayOneShot(Hey);
        }
        if (cameraTurn != null)
        {
            Debug.Log("calling camera turn");
            yield return StartCoroutine(cameraTurn.FaceNPC());
        }
        if (DialogueHandler.Instance != null)
        {
            dialogueType = DialogueType.Help;
            DialogueHandler.Instance.ShowDialogue(gameObject.name, helpDialoguetext);
        }
        yield return StartCoroutine(WaitForHelpChoice());
        if (SelectedChoiceIndex >= 0 && SelectedChoiceIndex < helpReply.Length)
        {
            string reply = helpReply[SelectedChoiceIndex];

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
        if (cameraTurn != null)
        {
            Debug.Log("calling camera turn");
            yield return StartCoroutine(cameraTurn.FaceComputer());
        }
        transform.position = originalReturnPosition;
        transform.rotation = originalReturnRotation;
        hasTeleported = true;
        if (wasTyping)
        {
            TriggerStartTyping();
        }

        isImmediateInteraction = false;
        isInteracting = false;
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
        if (audioSource != null && Hmm != null)
        {
            audioSource.PlayOneShot(Hmm);
        }
        if (DialogueHandler.Instance != null)
        {
            dialogueType = DialogueType.Idle;
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
        if (audioSource != null && Hmm != null)
        {
            audioSource.PlayOneShot(Hmm);
        }
        turnScript.SaveOriginalRotationPosition();
        TriggerStartSitting();
        yield return StartCoroutine(turnScript.TurnToPlayer());

        if (DialogueHandler.Instance != null)
        {
            dialogueType = DialogueType.Work;
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
    private IEnumerator WaitForHelpChoice()
    {
        SelectedChoiceIndex = 3;

        if (DialogueHandler.Instance != null)
        {
            DialogueHandler.Instance.ShowChoices(helpChoices, SetSelectedChoice);
            Debug.Log("trying to write choices");
        }
        yield return new WaitUntil(() => SelectedChoiceIndex != 3);
    }

    public void SetSelectedChoice(int choiceIndex)
    {
        SelectedChoiceIndex = choiceIndex;
    }
}
