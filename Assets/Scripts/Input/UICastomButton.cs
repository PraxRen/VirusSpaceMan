using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICastomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private Color _colorDefault;
    [SerializeField] private Color _colorDown;
    [SerializeField] private float _timeUpdateColor;

    private Coroutine _jobUpdateColor;

    public event Action ClickDown;
    public event Action ClickUp;

    private void OnValidate()
    {
        if (_image == null)
            return;

        _image.color = _colorDefault;
    }

    private void OnDisable()
    {
        CancelUpdateColor();
    }

    private IEnumerator UpdateColor(Color targetColor)
    {
        Color startColor = _image.color;
        float timer = 0f;

        while (timer < _timeUpdateColor)
        {
            timer += Time.deltaTime;
            _image.color = Color.Lerp(startColor, targetColor, timer / _timeUpdateColor);
            yield return null;
        }

        _image.color = targetColor;
        _jobUpdateColor = null;
    }

    private void CancelUpdateColor()
    {
        if (_jobUpdateColor != null)
        {
            StopCoroutine(_jobUpdateColor);
        }
    }

    private void RunUpdateColor(Color color)
    {
        CancelUpdateColor();
        StartCoroutine(UpdateColor(color));
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        RunUpdateColor(_colorDown);
        ClickDown?.Invoke();
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        RunUpdateColor(_colorDefault);
        ClickUp?.Invoke();
    }
}