using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleDEBUG : MonoBehaviour
{
    [SerializeField] private bool _canChangeValue;
    [Range(0f,1)][SerializeField] private float _value;

    private void Update()
    {
        if (_canChangeValue == false)
            return;

        Time.timeScale = _value;
    }
}