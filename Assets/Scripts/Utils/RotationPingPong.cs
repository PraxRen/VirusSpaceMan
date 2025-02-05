using UnityEngine;
using UnityEngine.UIElements;

public class RotationPingPong : MonoBehaviour
{
    [SerializeField] private Vector3 rotationAxis = Vector3.up;
    [SerializeField] private float maxAngle = 45f;
    [SerializeField] private float speed = 1f;

    private Transform _transform;
    private Quaternion _startRotation;

    private void Start()
    {
        _transform = transform;
        _startRotation = transform.localRotation;
    }

    private void Update()
    {
        float angle = Mathf.PingPong(Time.time * speed, maxAngle * 2) - maxAngle;
        Quaternion targetRotation = _startRotation * Quaternion.AngleAxis(angle, rotationAxis);
        _transform.localRotation = targetRotation;
    }
}