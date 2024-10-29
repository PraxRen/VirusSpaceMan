using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;

[AddComponentMenu("Input/Floating On-Screen Stick")]
public class FloatingOnScreenStick : OnScreenControl, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private float _movementRange = 50;
    [InputControl(layout = "Vector2")][SerializeField] private string _controlPath;
    [SerializeField] private RectTransform _stickRectTransform;
    [SerializeField] private RectTransform _knobRectTransform;
    [SerializeField] private Image _stickImage;
    [SerializeField] private Image _knobImage;
    [SerializeField] private Vector2 _sizeStick;

    private RectTransform _rectTransform;
    private Vector2 _startPosition;
    private Vector2 _pointerDownPosition;
    private Vector2 _dragPosition;

    protected override string controlPathInternal
    {
        get => _controlPath;
        set => _controlPath = value;
    }

    private void Awake()
    {
        _rectTransform = transform as RectTransform;

        if (_rectTransform == null)
            throw new InvalidCastException(nameof(_rectTransform));

        _startPosition = _rectTransform.anchoredPosition;
        _pointerDownPosition = _startPosition;
    }

    private Vector2 ClampPosition(Vector2 startPosition)
    {
        float limitWidth = (Screen.width - Math.Abs(_rectTransform.sizeDelta.x * _rectTransform.localScale.x)) / 2 - (_sizeStick.x / 2);
        float limitHeight = (Screen.height - Math.Abs(_rectTransform.sizeDelta.y * _rectTransform.localScale.y)) / 2 - (_sizeStick.y / 2);
        startPosition.x = Mathf.Clamp(startPosition.x, -limitWidth, limitWidth);
        startPosition.y = Mathf.Clamp(startPosition.y, -limitHeight, limitHeight);
        return startPosition;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (eventData == null)
            throw new ArgumentNullException(nameof(eventData));

        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, eventData.position, eventData.pressEventCamera, out _pointerDownPosition);
        _pointerDownPosition = ClampPosition(_pointerDownPosition);
        _stickRectTransform.anchoredPosition = _pointerDownPosition;
        _stickImage.enabled = true;
        _knobImage.enabled = true;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (eventData == null)
            throw new ArgumentNullException(nameof(eventData));

        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, eventData.position, eventData.pressEventCamera, out _dragPosition);
        Vector2 delta = _dragPosition - _pointerDownPosition;
        delta = Vector2.ClampMagnitude(delta, _movementRange);
        _knobRectTransform.anchoredPosition = delta;
        Vector2 newPosition = new Vector2(delta.x / _movementRange, delta.y / _movementRange);
        SendValueToControl(newPosition);
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        if (eventData == null)
            throw new ArgumentNullException(nameof(eventData));

        _stickImage.enabled = false;
        _knobImage.enabled = false;
        _stickRectTransform.anchoredPosition = _startPosition;
        _knobRectTransform.anchoredPosition = Vector2.zero;
        SendValueToControl(Vector2.zero);
    }
}