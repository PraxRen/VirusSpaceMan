using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class UICastomButton : MonoBehaviour, IReadOnlyButton, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private bool _isValidateBounds;

    public event Action ClickDown;
    public event Action ClickUp;
    public event Action ClickUpInBounds;

    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        ClickDown?.Invoke();
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        ClickUp?.Invoke();

        if (_isValidateBounds && RectTransformUtility.RectangleContainsScreenPoint(_rectTransform, eventData.position, eventData.pressEventCamera))
            ClickUpInBounds?.Invoke();
    }
}