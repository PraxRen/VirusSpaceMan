using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(IReadOnlyButton))]
public class UIButtonChangerImageColor : MonoBehaviour
{   
    [SerializeField] private Image _image;
    [SerializeField] private Color _colorDefault;
    [SerializeField] private Color _colorDown;
    [SerializeField] private float _timeUpdateColor;

    private IReadOnlyButton _button;
    private Coroutine _jobUpdateColor;

    private void OnValidate()
    {
        if (_image == null)
            return;

        _image.color = _colorDefault;
    }

    private void Awake()
    {
        _button = GetComponent<IReadOnlyButton>();
    }

    private void OnEnable()
    {
        _button.ClickDown += OnClickDown;
        _button.ClickUp += OnClickUp;
        _image.color = _colorDefault;
    }


    private void OnDisable()
    {
        _button.ClickDown -= OnClickDown;
        _button.ClickUp -= OnClickUp;
        CancelUpdateColor();
    }

    private void RunUpdateColor(Color color)
    {
        CancelUpdateColor();
        StartCoroutine(UpdateColor(color));
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

    private void OnClickDown()
    {
        RunUpdateColor(_colorDown);
    }

    private void OnClickUp()
    {
        RunUpdateColor(_colorDefault);
    }
}