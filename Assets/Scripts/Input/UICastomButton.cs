using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICastomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public event Action ClickDown;
    public event Action ClickUp;

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        ClickDown?.Invoke();
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        ClickUp?.Invoke();
    }
}