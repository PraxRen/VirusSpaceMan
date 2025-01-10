using UnityEngine;

public interface ITarget
{
    public Vector3 Position { get; }
    public Vector3 Center { get; }
    public Quaternion Rotation { get; }
    public Axis AxisUp { get; }
    public Axis AxisForward { get; }
    public Axis AxisRight { get; }

    public bool CanReach(Transform transform);
}