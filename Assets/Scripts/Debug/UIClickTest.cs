using UnityEngine;
using UnityEngine.EventSystems;

public class UIClickTest : MonoBehaviour
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Canvas clicked!");
    }
}
