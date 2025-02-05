using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationPerlinNoise : MonoBehaviour
{
    [SerializeField] private float _perlinNoiseTimeScale;
    [SerializeField] private float _amplitude;

    private Transform _transform;
    private Vector3 _shakeAngles;

    private void Awake()
    {
        _transform = transform;
    }

    private void Update()
    {
        UpdateShake();
        _transform.localEulerAngles = _shakeAngles;
    }

    private void UpdateShake()
    {
        float time = Time.time * _perlinNoiseTimeScale;
        _shakeAngles.x = Mathf.PerlinNoise(time, 0);
        _shakeAngles.y = Mathf.PerlinNoise(0, time);
        _shakeAngles.z = Mathf.PerlinNoise(time, time);
        _shakeAngles *= _amplitude;
    }
}