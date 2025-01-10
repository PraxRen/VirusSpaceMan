using System;
using UnityEngine;

public class Waypoint : MonoBehaviour, ITarget, ICloneable
{
    [SerializeField] private float _radius;
    
    private Transform _transform;

    public PatrolPath PatrolPath { get; private set; }
    public Vector3 Position => _transform.position;
    public Vector3 Center => _transform.position;
    public Quaternion Rotation => _transform.rotation;
    public Axis AxisUp => Axis.Y;
    public Axis AxisForward => Axis.Z;
    public Axis AxisRight => Axis.X;
    public float Radius => _radius;


    private void Awake()
    {
        _transform = transform;
    }

    public void Initialize(PatrolPath patrolPath)
    {
        PatrolPath = patrolPath;
    }

    public bool CanReach(Transform transform)
    {
        return (transform.position - _transform.position).sqrMagnitude < (_radius * _radius);
    }

    public object Clone()
    {
        Waypoint newWaypoint = Instantiate(this, transform.position, transform.rotation);
        newWaypoint._radius = _radius;
        newWaypoint._transform = newWaypoint.transform;
        newWaypoint.PatrolPath = PatrolPath;
        return newWaypoint;
    }
}