using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RigHit : MonoBehaviour
{
    [SerializeField] private Rig _rig;
    [SerializeField] private Transform _target;
    [SerializeField] private float _timeUpdateRigWeight;

    private Coroutine _jobUpdateRigWeight;
    private Vector3 _startPositionTarget;

    private void Awake()
    {
        _startPositionTarget = _target.localPosition;
    }

    public void AddForce(Vector3 forceDirection)
    {
        if (_jobUpdateRigWeight != null)
            StopCoroutine(_jobUpdateRigWeight);

        _target.localPosition = _startPositionTarget;
        _target.position += forceDirection;

        _jobUpdateRigWeight = StartCoroutine(UpdateRigWeight(1, () =>
        {
            _jobUpdateRigWeight = StartCoroutine(UpdateRigWeight(0, () => _target.localPosition = _startPositionTarget));
        }));
    }

    private IEnumerator UpdateRigWeight(float targetValue, Action actionComplete)
    {
        float startValue = _rig.weight;
        targetValue = Mathf.Clamp01(targetValue);
        float elapsedTime = 0f;
        float delta = Mathf.Abs(targetValue - startValue);
        float speedUpdate = delta / _timeUpdateRigWeight;

        while (Mathf.Approximately(_rig.weight, targetValue) == false)
        {
            elapsedTime += Time.deltaTime;
            _rig.weight = Mathf.Lerp(startValue, targetValue, speedUpdate * elapsedTime / delta);
            yield return null;
        }

        _rig.weight = targetValue;
        _jobUpdateRigWeight = null;
        actionComplete?.Invoke();
    }
}