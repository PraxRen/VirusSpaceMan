using System;
using System.Collections;
using UnityEngine;

public class Rage : MonoBehaviour, IAttribute
{
    [SerializeField][ReadOnly] private float _value;
    [SerializeField] private float _maxValue;
    [SerializeField] private float _speedDecreaseValue;
    [SerializeField] private float _timeWaitForMaxValue;

    private Coroutine _jobUpdateDecreaseValue;
    private Coroutine _jobWaitUpdateDecreaseValue;
    private WaitForSeconds _waitForMaxValue;

    public float MaxValue => _maxValue;
    public float Value => _value;

    public event Action ValueChanged;
    public event Action ValueMaxAchieved;

    private void Awake()
    {
        _waitForMaxValue = new WaitForSeconds(_timeWaitForMaxValue);
    }

    private void OnDisable()
    {
        Cancel();
    }

    public void AddPoint(float points)
    {
        if (points < 0)
            throw new IndexOutOfRangeException(nameof(points));

        UpdateValue(points);

        if (_value == _maxValue)
        {
            StopUpdateDecreaseValue();
            ValueMaxAchieved?.Invoke();
            _jobWaitUpdateDecreaseValue = StartCoroutine(WaitUpdateDecreaseValue());
            return;
        }

        StartUpdateDecreaseValue();
    }

    public void ReaplacePoints(float points)
    {
        if (points < 0)
            throw new IndexOutOfRangeException(nameof(points));

        StopWaitUpdateDecreaseValue();
        UpdateValue(-points);
    }

    public void Cancel()
    {
        StopUpdateDecreaseValue();
        ReaplacePoints(_maxValue);
    }

    private IEnumerator UpdateDecreaseValue()
    {
        while (_value > 0)
        {
            UpdateValue(-_speedDecreaseValue * Time.deltaTime);
            yield return null;
        }

        _jobUpdateDecreaseValue = null;
    }

    private IEnumerator WaitUpdateDecreaseValue()
    {
        yield return _waitForMaxValue;
        StartUpdateDecreaseValue();
        _jobWaitUpdateDecreaseValue = null;
    }

    private void UpdateValue(float points)
    {
        _value = Mathf.Clamp(_value + points, 0, _maxValue);
        ValueChanged?.Invoke();
    }

    private void StartUpdateDecreaseValue()
    {
        if (_jobUpdateDecreaseValue != null)
            return;

        _jobUpdateDecreaseValue = StartCoroutine(UpdateDecreaseValue());
    }

    private void StopUpdateDecreaseValue()
    {
        if (_jobUpdateDecreaseValue == null)
            return;

        StopCoroutine(_jobUpdateDecreaseValue);
        _jobUpdateDecreaseValue = null;
    }

    private void StopWaitUpdateDecreaseValue()
    {
        if (_jobWaitUpdateDecreaseValue == null)
            return;

        StopCoroutine(_jobWaitUpdateDecreaseValue);
        _jobWaitUpdateDecreaseValue = null;
    }
}