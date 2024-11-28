using System;
using UnityEngine;

public class Health : MonoBehaviour, IHealth, IDamageable, IAttribute, IAction
{
    [SerializeField] private float _maxValue;
    [SerializeField] private float _cooldownHit;
    [SerializeField] private ActionScheduler _actionScheduler;
    [Header(nameof(ISurface))]
    [SerializeField] private float _factorNoise = 1f;
    [SerializeField] private SurfaceType _surfaceType;
    [Header(nameof(ITarget))]
    [SerializeField] private float _radiusCanReachTarget = 0.2f;

    private Transform _transform;
    private float _lastTimeHit;

    public Vector3 Position => _transform.position;
    public Quaternion Rotation => _transform.rotation;
    public float MaxValue => _maxValue;
    public float Value { get; private set; }
    public bool IsDied { get; private set; }
    public float FactorNoise => _factorNoise;
    public SurfaceType SurfaceType => _surfaceType;

    public event Action ValueChanged;
    public event Action Died;
    public event Action<IWeaponReadOnly, Vector3, float> BeforeTakeDamage;
    public event Action<IWeaponReadOnly, Vector3, float> AfterTakeDamage;

    private void Awake()
    {
        _transform = transform;
    }

    private void Start()
    {
        UpdateValue(_maxValue);
    }

    public void Cancel() { }

    public bool CanReach(Transform transform) 
    {
        return (transform.position - _transform.position).sqrMagnitude > (_radiusCanReachTarget * _radiusCanReachTarget);
    }

    public bool CanTakeDamage(IWeaponReadOnly weapon)
    {
        if (weapon == null)
            throw new ArgumentNullException(nameof(weapon));

        if (IsDied)
            return false;

        if (Time.time - _lastTimeHit < _cooldownHit)
            return false;

        return true;
    }

    public void TakeDamage(IWeaponReadOnly weapon, Vector3 hitPoint, float damage)
    {
        if (weapon == null)
            throw new ArgumentNullException(nameof(weapon));

        if (damage < 0)
            throw new ArgumentOutOfRangeException(nameof(damage));

        BeforeTakeDamage?.Invoke(weapon, hitPoint, damage);
        UpdateValue(Value - damage);

        if (Value == 0)
            Die();

        AfterTakeDamage?.Invoke(weapon, hitPoint, damage);
    }

    public bool CanDie(IWeaponReadOnly weapon, float damage)
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