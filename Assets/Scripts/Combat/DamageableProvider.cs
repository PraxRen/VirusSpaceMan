using System;
using Unity.VisualScripting;
using UnityEngine;

public class DamageableProvider : MonoBehaviour, IDamageable
{
    [SerializeField][SerializeInterface(typeof(IDamageable))] private MonoBehaviour _mainDamageableMonoBehaviour;
    [SerializeField] private Collider _collider;
    [SerializeField] private Vector3 _offsetCenterCollider;
    [SerializeField] private Axis _axisUp = Axis.Y;
    [SerializeField] private Axis _axisForward = Axis.Z;
    [SerializeField] private Axis _axisRight = Axis.X;

    private Transform _transform;
    private IDamageable _mainDamageable;

    public event Action<Hit, float> BeforeTakeDamage;
    public event Action<Hit, float> AfterTakeDamage;

    public Vector3 Position => _transform.position;
    public Vector3 Center => _collider.bounds.center + _offsetCenterCollider;
    public Quaternion Rotation => _transform.rotation;
    public Axis AxisUp => _axisUp;
    public Axis AxisForward => _axisForward;
    public Axis AxisRight => _axisRight;
    public float FactorNoise => _mainDamageable.FactorNoise;
    public SurfaceType SurfaceType => _mainDamageable.SurfaceType;

    private void Awake()
    {
        _transform = transform;
        _mainDamageable = (IDamageable)_mainDamageableMonoBehaviour;
        AwakeAddon();
    }

    public bool CanReach(Transform transform) => _mainDamageable.CanReach(transform);

    public bool CanDie(Hit hit, float damage) => _mainDamageable.CanDie(hit, damage);

    public bool CanTakeDamage() => _mainDamageable.CanTakeDamage();

    public void TakeDamage(Hit hit, float damage) 
    {
        BeforeTakeDamage?.Invoke(hit, damage);
        _mainDamageable.TakeDamage(hit, damage);
        AfterTakeDamage?.Invoke(hit, damage);
    }

    protected virtual void AwakeAddon() { }
}