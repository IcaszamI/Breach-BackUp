using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeSittingInteraction : MonoBehaviour
{
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
    [Header("Power")]
    public GameObject power;
    [Header("Screen Cursor Script")]
    public ScreenCursor screenCursor;
    public HomeEmailManager emailManager;
    public float interactionDistance = 1f;
    public KeyCode sit = KeyCode.F;
    public bool isSitting = false;
    public GameObject laptopScreenOff;

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
            EmailUI?.SetActive(false);
            power?.SetActive(false);
        }
        laptopScreenOff?.SetActive(false);

        if (playerController != null) playerController.enabled = false;
        screenCursor.enabled = true;
        Debug.Log("tried to enabled");
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
        laptopScreenOff?.SetActive(true);
        if (playerController != null) playerController.enabled = true;
        screenCursor.enabled = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
