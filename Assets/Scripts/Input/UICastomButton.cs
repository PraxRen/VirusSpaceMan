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

    public void Down()
    {
        ClickDown?.Invoke();
    }

    public void Up(bool isInsideBorders = false)
    {
        ClickUp?.Invoke();

        if (_isValidateBounds && isInsideBorders)
            ClickUpInBounds?.Invoke();
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        Down();
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        Up(RectTransformUtility.RectangleContainsScreenPoint(_rectTransform, eventData.position, eventData.pressEventCamera));
    }
}