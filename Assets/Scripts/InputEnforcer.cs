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
        }
    }


    private void LateUpdate()
    {
        if (Time.timeScale == 0f)
        {
            return;
        }
        if (Cursor.lockState != CursorLockMode.Locked && !interacting.currentlyInteracting)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (playerController != null && !playerController.enabled && !interacting.currentlyInteracting)
        {
            playerController.enabled = true;
        }
    }
    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus && Time.timeScale > 0f)
        {
            StartCoroutine(EnsureLockedAfterFocus());
        }
    }

    private IEnumerator EnsureLockedAfterFocus()
    {
        yield return null; 
        
        if (Time.timeScale > 0f)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            if (playerController != null)
                playerController.enabled = true;
        }
    }
}
