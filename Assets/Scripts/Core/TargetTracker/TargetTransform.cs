using UnityEngine;

public class TargetTransform : ITarget
{
    private readonly Transform _transform;
    private readonly float _radius;

    public TargetTransform(Transform transform, float radius)
    {
        _transform = transform;
        _radius = radius;
    }

    public Vector3 Position => _transform.position;
    public Quaternion Rotation => _transform.rotation;

    public bool CanReach(Transform transform) 
    {
        return (transform.position - _transform.position).sqrMagnitude < (_radius * _radius);
    }
}
