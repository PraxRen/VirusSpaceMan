using System;
using UnityEngine;

public class TimeScaler : MonoBehaviour
{
    private IChangerTime _currentChangerTime;

    public void Play(IChangerTime changerTime) => SetScale(changerTime, 1f);

    public void Pause(IChangerTime changerTime) => SetScale(changerTime, 0f);

    public void SetScale(IChangerTime changerTime, float value)
    {
        if (_currentChangerTime != null && _currentChangerTime != changerTime)
            return;

        _currentChangerTime = changerTime;
        value = Mathf.Clamp01(value);
        Time.timeScale = value;
    }

    public void ClearChangerTime(IChangerTime changerTime)
    {
        if (_currentChangerTime != null && _currentChangerTime != changerTime)
            throw new InvalidOperationException($"{nameof(changerTime)} is not {_currentChangerTime}");

        _currentChangerTime = null;
    }
}