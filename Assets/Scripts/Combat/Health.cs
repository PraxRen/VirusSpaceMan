using System;
using System.Linq;
using UnityEngine;

public class Health : MonoBehaviour, IHealth, IDamageable, IAttribute, IAction
{
    [SerializeField] private float _maxValue;
    [SerializeField] private float _cooldownHit;
    [Header(nameof(IAction))]
    [SerializeField] private ActionScheduler _actionScheduler;
    [Header(nameof(ISurface))]
    [SerializeField] private float _factorNoise = 1f;
    [SerializeField] private SurfaceType _surfaceType;
    [Header(nameof(ITarget))]
    [SerializeField] private float _radiusCanReachTarget = 0.3f;
    [SerializeField] private Collider _collider;
    [SerializeField] private Vector3 _offsetCenterCollider;
    [SerializeField][SerializeInterface(typeof(IHandlerSelectionTarget))] private MonoBehaviour[] _handlersSelectionTargetMonoBehaviour;

    private Transform _transform;
    private float _lastTimeHit;
    private IHandlerSelectionTarget[] _handlerSelectionTarget;

    public Vector3 Position => _transform.position;
    public Vector3 Center => _collider.bounds.center + _offsetCenterCollider;
    public Quaternion Rotation => _transform.rotation;
    public Axis AxisUp => Axis.Y;
    public Axis AxisForward => Axis.Z;
    public Axis AxisRight => Axis.X;
    public float MaxValue => _maxValue;
    public float Value { get; private set; }
    public bool IsDied { get; private set; }
    public float FactorNoise => _factorNoise;
    public SurfaceType SurfaceType => _surfaceType;

    public event Action ValueChanged;
    public event Action Died;
    public event Action<Hit, float> BeforeTakeDamage;
    public event Action<Hit, float> AfterTakeDamage;

    private void Awake()
    {
        _transform = transform.parent;
        _handlerSelectionTarget = _handlersSelectionTargetMonoBehaviour.Cast<IHandlerSelectionTarget>().ToArray();
    }

    private void Start()
    {
        UpdateValue(_maxValue);
    }

    public void HandleSelection()
    {
        foreach (IHandlerSelectionTarget handler in _handlerSelectionTarget) 
            handler.Select();
    }

    public void HandleDeselection()
    {
        foreach (IHandlerSelectionTarget handler in _handlerSelectionTarget) 
            handler.Deselect();
    }

    public void Cancel() { }

    public bool CanReach(Transform transform) 
    {
        return (transform.position - _transform.position).sqrMagnitude < (_radiusCanReachTarget * _radiusCanReachTarget);
    }

    public bool CanTakeDamage()
    {
        if (IsDied)
            return false;

        if (Time.time - _lastTimeHit < _cooldownHit)
            return false;

        return true;
    }

    public void TakeDamage(Hit hit, float damage)
    {
        if (damage < 0)
            throw new ArgumentOutOfRangeException(nameof(damage));

        BeforeTakeDamage?.Invoke(hit, damage);
        UpdateValue(Value - damage);

        if (Value == 0)
            Die();

        AfterTakeDamage?.Invoke(hit, damage);
    }

    public bool CanDie(Hit hit, float damage)
    {
        return (Value - damage) > 0 ? false : true;
    }

    private void UpdateValue(float value)
    {
        Value = Mathf.Clamp(value, 0, _maxValue);
        _lastTimeHit = Time.time;
        ValueChanged?.Invoke();
    }

    private void Die()
    {
        if (IsDied)
            return;

        IsDied = true;
        _actionScheduler.StartAction(this);
        _actionScheduler.SetBlock(this);
        Died?.Invoke();
    }
}