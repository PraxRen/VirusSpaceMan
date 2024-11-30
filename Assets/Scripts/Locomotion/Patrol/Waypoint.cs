using UnityEngine;

public class Waypoint : MonoBehaviour, ITarget
{
    [SerializeField] private float _radius;
    
    private Transform _transform;

    public PatrolPath PatrolPath { get; private set; }
    public Vector3 Position => _transform.position;
    public Quaternion Rotation => _transform.rotation;
    public float Radius => _radius;

    public void Initialize(PatrolPath patrolPath)
    {
        PatrolPath = patrolPath;
    }

    public bool CanReach(Transform transform)
    {
        return (transform.position - _transform.position).sqrMagnitude > (_radius * _radius);
    }
}