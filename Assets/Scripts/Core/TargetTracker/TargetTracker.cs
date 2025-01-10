using UnityEngine;

public class TargetTracker : MonoBehaviour, IReadOnlyTargetTracker
{
    [SerializeField] private Transform _pointDefault;
    [SerializeField] private float _speedUpdatePositionDefault;
    [SerializeField] private float _speedUpdatePositionTarget;

#if UNITY_EDITOR
    [Header("Gizmos")]
    [SerializeField] private Color _color;
    [SerializeField] private float _size;
    [Header("Debug")]
    [ReadOnly][SerializeField] private string _targetName;
#endif

    private Transform _transform;
    private TargetTransform _targetDefault;
    private Vector3 _offset;
    private float _speedUpdate;

    public Vector3 Position => _transform.position;
    public ITarget Target { get; private set; }

    private void OnDrawGizmos()
    {
        Gizmos.color = _color;
        Gizmos.DrawSphere(transform.position, _size);
    }

    private void Awake()
    {
        _transform = transform;
        _targetDefault = new TargetTransform(_pointDefault, 0f);
        _transform.parent = null;
    }

    private void OnEnable()
    {
        ResetTarget();
        _transform.position = _pointDefault.position;
    }

    public void Update()
    {
        _transform.position = Vector3.MoveTowards(_transform.position, Target.Position + CalculateOffset(), _speedUpdate * Time.deltaTime);
    }

    public void SetTarget(ITarget target, Vector3 offset)
    {
        Target = target;
        _offset = offset;
        _speedUpdate = _speedUpdatePositionTarget;
#if UNITY_EDITOR
        MonoBehaviour monoBehaviour = Target as MonoBehaviour;
        _targetName = monoBehaviour == null ? "" : monoBehaviour.GetType().Name;
#endif
    }

    public void ResetTarget()
    {
        Target = _targetDefault;
        _offset = Vector3.zero;
        _speedUpdate = _speedUpdatePositionDefault;
#if UNITY_EDITOR
        MonoBehaviour monoBehaviour = Target as MonoBehaviour;
        _targetName = monoBehaviour == null ? "" : monoBehaviour.GetType().Name;
#endif
    }


    private Vector3 CalculateOffset()
    {
        return (Target.Rotation * SimpleUtils.GetVectorDirection(Target.AxisRight)) * _offset.x 
             + (Target.Rotation * SimpleUtils.GetVectorDirection(Target.AxisUp)) * _offset.y 
             + (Target.Rotation * SimpleUtils.GetVectorDirection(Target.AxisForward)) * _offset.z;
    }
}