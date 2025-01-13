using UnityEngine;

public class ScaleModifier : MonoBehaviour
{
    [Range(0f, 1f)][SerializeField] private float _minFactorScale;
    [Range(1f, 10f)][SerializeField] private float _maxFactorScale;
    [SerializeField] private float _speed;

    private Transform _transform;
    private Vector3 _defaultScale;
    private Vector3 _targetScaleMax;
    private Vector3 _targetScaleMin;
    private Vector3 _targetScale;

    private void Awake()
    {
        _transform = transform;
        _defaultScale = transform.localScale;
        _targetScaleMax = _defaultScale * _maxFactorScale;
        _targetScaleMin = _defaultScale * _minFactorScale;
    }

    private void OnEnable()
    {
        _transform.localScale = _defaultScale;
        _targetScale = _targetScaleMax;
    }

    private void Update()
    {
        if (Mathf.Approximately(_transform.localScale.sqrMagnitude, _targetScale.sqrMagnitude))
        {
            _targetScale = _targetScale == _targetScaleMax ? _targetScaleMin : _targetScaleMax;
        }

        _transform.localScale = Vector3.MoveTowards(_transform.localScale, _targetScale, _speed * Time.deltaTime); 
    }
}