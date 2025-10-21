using UnityEngine;
using StarterAssets;
using System.Collections;

public class InputEnforcer : MonoBehaviour
{
    public FirstPersonController playerController;
    public InteractionManager interacting;

    private void Awake()
    {
        if (playerController == null)
        {
            playerController = GetComponent<FirstPersonController>();
        }
        if (playerController == null)
        {
            Debug.LogError("No FirstPersonController Found");
            enabled = false;
            return;
        }

        ForceLock();
    }

    private void ForceLock()
    {
        if (Time.timeScale == 0f || interacting == null || playerController == null)
        {
            return;
        }

        if (interacting.currentlyInteracting)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (playerController != null && !playerController.enabled)
            {
                playerController.enabled = false;
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (!playerController.enabled)
            {
                playerController.enabled = true;
            }
        }
    }

    private void Start()
    {
        StartCoroutine(EnsureLockedAfterFocus());
    }

    private void LateUpdate()
    {
        if (Time.timeScale == 0f)
        {
            return;
        }
        ForceLock();
    }
    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            StartCoroutine(EnsureLockedAfterFocus());
        }
    }

    private IEnumerator EnsureLockedAfterFocus()
    {
        yield return null;
        ForceLock();
    }
}
