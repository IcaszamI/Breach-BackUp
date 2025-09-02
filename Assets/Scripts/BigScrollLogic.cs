using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BigScrollLogic : MonoBehaviour
{
    public float scrolSpeed = 50f;
    private RectTransform rectTransform;
    private RectTransform parentRect;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentRect = transform.parent.GetComponent<RectTransform>();
    }
    void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            rectTransform.anchoredPosition -= new Vector2(0, scrollInput * scrolSpeed);
        }
        ClampContent();
    }

    void ClampContent()
    {
        if (rectTransform == null || parentRect == null) return;
        Vector2 anchoredPos = rectTransform.anchoredPosition;
        float contentHeight = rectTransform.rect.height;
        float viewportHeight = parentRect.rect.height;
        if (contentHeight <= viewportHeight)
        {
            anchoredPos.y = 0;
        }
        else
        {
            float minY = 0;
            float maxY = contentHeight - viewportHeight;
            anchoredPos.y = Mathf.Clamp(anchoredPos.y, minY, maxY);
        }

        rectTransform.anchoredPosition = anchoredPos;
    }
}
