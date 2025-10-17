using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using StarterAssets;
using UnityEngine;

public class SleepInterationHandler : MonoBehaviour
{
    public FirstPersonController playerController;
    public Transform player;
    public Transform bed;
    public GameObject prompt;
    public GameObject daylights;
    public float interactionDistance = 4f;
    public KeyCode interact = KeyCode.F;
    private AudioSource audioSource;
    public AudioClip Yawn;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }


    void Update()
    {
        if (GameManager.Instance.AfterHours)
        {
            daylights?.SetActive(false);
        }
        else
        {
            daylights?.SetActive(true);
        }
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= interactionDistance && GameManager.Instance.AfterHours)
        {
            if (prompt != null)
            {
                prompt.SetActive(true);
            }
            if (Input.GetKeyDown(interact))
            {
                StartCoroutine(SleepSequence());
            }
        }
        else
        {
            prompt?.gameObject.SetActive(false);
        }
    }

    private IEnumerator SleepSequence()
    {
        if (audioSource != null && Yawn != null)
        {
            audioSource.PlayOneShot(Yawn);
        }
        yield return new WaitForSeconds(3f);
        GameManager.Instance.StartNextDay();
    }
}
