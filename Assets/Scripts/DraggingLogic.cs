using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggingLogic : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    public RectTransform windowDrag;
    public RectTransform bounds;
    public Canvas canvas;
    private CanvasGroup canvasGroup;
    private bool isDragging = false;

    void Awake()
    {
        if (GetComponent<CanvasGroup>() == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        else
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.8f;
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (windowDrag != null || canvas != null)
        {
            windowDrag.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        isDragging = false;
    }

    void LateUpdate()
    {
        if (isDragging)
        {
            ClampWindow();
        }
    }

    void ClampWindow()
    {
        if (windowDrag == null || bounds == null) return;
        Vector2 windowSize = windowDrag.rect.size;
        Vector2 boundsSize = bounds.rect.size;
        Vector2 anchoredPos = windowDrag.anchoredPosition;
        float minX = (boundsSize.x * -0.5f) + (windowSize.x * windowDrag.pivot.x);
        float maxX = (boundsSize.x * 0.5f) + (windowSize.x * (1 - windowDrag.pivot.x));

        float minY = (boundsSize.y * -0.5f) + (windowSize.y * windowDrag.pivot.y);
        float maxY = (boundsSize.y * 0.5f) + (windowSize.y * (1 - windowDrag.pivot.y));

        anchoredPos.x = Mathf.Clamp(anchoredPos.x, minX, maxX);
        anchoredPos.y = Mathf.Clamp(anchoredPos.y, minY, maxY);

        windowDrag.anchoredPosition = anchoredPos;

    }
}
