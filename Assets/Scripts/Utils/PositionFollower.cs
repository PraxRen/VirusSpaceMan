using UnityEngine;

public class PositionFollower : MonoBehaviour
{
    [SerializeField] private Transform _transformTarget;

    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    private void Update()
    {
        _transform.position = _transformTarget.position;
    }
}