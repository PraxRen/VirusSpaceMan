using UnityEngine;
using UnityEngine.UI;

public class UIRadiusScanner : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private float _factorSize;
    [SerializeField][SerializeInterface(typeof(IReadOnlyScanner))] private MonoBehaviour _scannerMonoBehaviour;

    private IReadOnlyScanner _scanner;
    private RectTransform _imageTransform;

    private void Awake()
    {
        _imageTransform = _image.transform as RectTransform;
        _scanner = (IReadOnlyScanner)_scannerMonoBehaviour;
    }

    private void OnEnable()
    {
        _scanner.ChangedRadius += OnChangedRadius;
    }

    private void OnDisable()
    {
        _scanner.ChangedRadius -= OnChangedRadius;
    }

    private void OnChangedRadius(float radius)
    {
        _imageTransform.sizeDelta = Vector2.one * radius * _factorSize;
    }
}