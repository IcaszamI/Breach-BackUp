using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScreenCursor : MonoBehaviour
{
    public Camera uiCamera;
    public RectTransform virtualCursor;
    public RectTransform screenBounds;
    public GraphicRaycaster raycaster;
    public EventSystem eventSystem;
    private DraggingLogic drag;
    private PointerEventData pointerEventData;
    public float cursorSpeed = 0.1f;

    void OnEnable()
    { if (virtualCursor != null)
        {
            Debug.Log("Vcursor enabled");
            virtualCursor.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void OnDisable()
    {
        if (virtualCursor != null)
        {
            virtualCursor.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        MoveCursor();
        HandleInteractions();
    }

    void MoveCursor()
    {
        float mouseX = Input.GetAxis("Mouse X") * cursorSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * cursorSpeed;
        virtualCursor.anchoredPosition += new Vector2(mouseX, mouseY);
        Vector2 halfsize = screenBounds.rect.size / 2;
        Vector2 clampedPosition = virtualCursor.anchoredPosition;
        clampedPosition.x = Mathf.Clamp(virtualCursor.anchoredPosition.x, -halfsize.x, halfsize.x);
        clampedPosition.y = Mathf.Clamp(virtualCursor.anchoredPosition.y, -halfsize.y, halfsize.y);
        virtualCursor.anchoredPosition = clampedPosition;
    }

    void HandleInteractions()
    {
        pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = uiCamera.WorldToScreenPoint(virtualCursor.position);
        if (Input.GetMouseButtonDown(0))
        {
            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerEventData, results);
            foreach (RaycastResult result in results)
            {
                DraggingLogic dragHandle = result.gameObject.GetComponent<DraggingLogic>();
                if (dragHandle != null)
                {
                    drag = dragHandle;
                    drag.OnBeginDrag(pointerEventData);
                    break;
                }
                Button button = result.gameObject.GetComponent<Button>();
                if (button != null)
                {
                    button.onClick.Invoke();
                    break;
                }
            }
        }

        if (drag != null)
        {
            drag.OnDrag(pointerEventData);
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (drag != null)
            {
                drag.OnEndDrag(pointerEventData);
                drag = null;
            }
        }
        
    }

    

}
