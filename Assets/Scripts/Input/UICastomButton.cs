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
    public event Action Activated;
    public event Action Deactivated;

    private RectTransform _rectTransform;
    private bool _isActivated = true;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void Activate()
    {
        _isActivated = true;
        Activated?.Invoke();
    }

    public void Deactivate()
    {
        _isActivated = false;
        Deactivated?.Invoke();
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
        if (_isActivated == false)
            return;

        Down();
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        if (_isActivated == false)
            return;

        Up(RectTransformUtility.RectangleContainsScreenPoint(_rectTransform, eventData.position, eventData.pressEventCamera));
    }
}