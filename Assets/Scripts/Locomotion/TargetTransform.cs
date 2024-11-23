using UnityEngine;

public class TargetTransform : ITarget
{
    private readonly Transform _transform;

    public TargetTransform(Transform transform)
    {
        _transform = transform;
    }

    public Vector3 Position => _transform.position;
    public Quaternion Rotation => _transform.rotation;
}
