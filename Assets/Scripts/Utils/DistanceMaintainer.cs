using UnityEngine;

public class DistanceMaintainer : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _distance;
    [SerializeField] private float _inaccuracy;

    private Transform _transform;
    private float _sqrDistance;

    private void Awake()
    {
        _sqrDistance = _distance * _distance;
        _transform = transform;
    }

    private void Update()
    {
        Vector3 vectorDistance = _transform.position - _target.position;
        float currentDistanceSqr = vectorDistance.sqrMagnitude;

        if (Mathf.Abs(currentDistanceSqr - _sqrDistance) > _inaccuracy)
        {
            Vector3 direction = vectorDistance.normalized;
            _transform.position = _target.position + direction * _distance;
        }
    }
}