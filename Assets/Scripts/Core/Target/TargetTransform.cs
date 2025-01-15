using TMPro.EditorUtilities;
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
    public Vector3 Center => _transform.position;
    public Quaternion Rotation => _transform.rotation;
    public Axis AxisUp => Axis.Y;
    public Axis AxisForward => Axis.Z;
    public Axis AxisRight => Axis.X;

    public void HandleSelection() { }

    public void HandleDeselection() { }

    public bool CanReach(Transform transform) 
    {
        return (transform.position - _transform.position).sqrMagnitude < (_radius * _radius);
    }
}
