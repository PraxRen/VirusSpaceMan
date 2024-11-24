using UnityEngine;

public interface ITarget
{
    public Vector3 Position { get; }
    public Quaternion Rotation { get; }
}