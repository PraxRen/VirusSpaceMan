using UnityEngine;

public class LookTarget : MonoBehaviour, IReadOnlyLookTarget
{
    [SerializeField] private Transform _parent;
    [SerializeField] private Transform _pointDefault;
    [SerializeField] private float _speedUpdatePositionDefault;
    [SerializeField] private float _speedUpdatePositionTarget;

    private Transform _transform;
    private Transform _target;
    private Vector3 _offset;
    private float _speedUpdate;

    public Vector3 Position => _transform.position;

    private void Awake()
    {
        _transform = transform;
    }

    private void OnEnable()
    {
        ResetTarget();
        _transform.position = _pointDefault.position;
        _transform.parent = null;
    }

    public void Update()
    {
        _transform.position = Vector3.MoveTowards(_transform.position, _target.position + _offset, _speedUpdate * Time.deltaTime);
    }

    public void SetTarget(Transform transform, Vector3 offset)
    {
        _target = transform;
        _offset = offset;
        _speedUpdate = _speedUpdatePositionTarget;
    }

    public void ResetTarget()
    {
        _target = _pointDefault;
        _offset = Vector3.zero;
        _speedUpdate = _speedUpdatePositionDefault;
    }
}