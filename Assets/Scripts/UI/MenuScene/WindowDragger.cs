using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowDragger : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public RectTransform windowRect;
    public Canvas canvas;

    Vector2 offset;

    public void ResetWindowRectPosition()
    {
       windowRect.anchoredPosition = Vector2.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != 0)
        {
            return;
        }

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, canvas.worldCamera, out Vector2 localPointerPosition);

        offset = windowRect.anchoredPosition - localPointerPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button != 0)
        {
            return; 
        }

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, canvas.worldCamera, out Vector2 localPointerPosition))
        {
            windowRect.anchoredPosition = localPointerPosition + offset;
        }
    }
}
