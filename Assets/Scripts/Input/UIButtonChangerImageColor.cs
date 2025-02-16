using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonChangerImageColor : MonoBehaviour
{
    [SerializeField][SerializeInterface(typeof(IReadOnlyButton))] private MonoBehaviour _buttonMonoBehaviour;
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
        _button = (IReadOnlyButton)_buttonMonoBehaviour;
    }

    private void OnEnable()
    {
        _button.ClickDown += OnClickDown;
        _button.ClickUp += OnClickUp;
        _button.Activated += OnActivated;
        _button.Deactivated += OnDeactivated;
        _image.color = _colorDefault;
    }

    private void OnDisable()
    {
        _button.ClickDown -= OnClickDown;
        _button.ClickUp -= OnClickUp;
        _button.Activated -= OnActivated;
        _button.Deactivated -= OnDeactivated;
        CancelUpdateColor();
    }

    public void ResetColors(Color colorDefault, Color colorDown)
    {
        _colorDefault = colorDefault;
        _colorDown = colorDown;
        _image.color = _colorDefault;
    }

    private void RunUpdateColor(Color color, float time)
    {
        CancelUpdateColor();
        StartCoroutine(UpdateColor(color, time));
    }

    private IEnumerator UpdateColor(Color targetColor, float timeUpdate)
    {
        Color startColor = _image.color;
        float timer = 0f;

        while (timer < timeUpdate)
        {
            timer += Time.deltaTime;
            _image.color = Color.Lerp(startColor, targetColor, timer / timeUpdate);
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
        RunUpdateColor(_colorDown, _timeUpdateColor);
    }

    private void OnClickUp()
    {
        RunUpdateColor(_colorDefault, _timeUpdateColor);
    }

    private void OnActivated()
    {
        RunUpdateColor(_colorDefault, 0f);
    }

    private void OnDeactivated()
    {
        RunUpdateColor(_colorDown, 0f);
    }
}