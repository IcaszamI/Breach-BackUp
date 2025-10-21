using UnityEngine.SceneManagement;
using UnityEngine;


public class CursorManager : MonoBehaviour
{
    public SittingInteraction sit;
    [Header("PauseUI")]
    public GameObject PauseCanvas;
    [Header("SettingsUI")]
    public GameObject SettingsCanvas;
    public static CursorManager instance;
    public NPCcontroller[] npcController;

    public bool isPaused = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        ForceCursorState();
    }

    void Update()
    {
        if (PauseCanvas.activeSelf || SettingsCanvas.activeSelf || isPaused)
        {
            SetCursor(CursorLockMode.None, true);
            return;
        }

        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "MainMenu" || currentScene == "Settings" || currentScene == "NextDayScene" || currentScene == "RepeatDayScene")
        {
            SetCursor(CursorLockMode.None, true);
        }

        foreach (NPCcontroller npc in npcController)
        {
            if (npc.isInteracting && npc.hasTeleported)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        if (sit != null && sit.isSitting)
        {
            SetCursor(CursorLockMode.None, true);
            return;
        }

        SetCursor(CursorLockMode.Locked, false);

    }
    private void SetCursor(CursorLockMode mode, bool visible)
    {
        Cursor.lockState = mode;
        Cursor.visible = visible;
    }

    private void ForceCursorState()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "MainMenu" || currentScene == "Settings" || currentScene == "NextDayScene" || currentScene == "RepeatDayScene")
        {
            SetCursor(CursorLockMode.None, true);
        }
        else
        {
            SetCursor(CursorLockMode.Locked, false);
        }
    }

    public void SetPaused(bool pause)
    {
        isPaused = pause;
    }

} 
