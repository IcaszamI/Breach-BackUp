using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualCursorInputModule : StandaloneInputModule
{
    public RectTransform mobileCursorRect;

    protected override MouseState GetMousePointerEventData(int id)
    {
        MouseState mouseState = base.GetMousePointerEventData(id);
        if (mobileCursorRect != null)
        {
            PointerEventData pointerData = mouseState.GetButtonState(PointerEventData.InputButton.Left).eventData.buttonData;
            Vector3 worldPos = mobileCursorRect.position;
            //Camera canvasCamera = GetComponent<Canvas>().worldCamera;
            Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(null, worldPos);
            pointerData.position = screenPosition;
        }
        return mouseState;
    }
}
