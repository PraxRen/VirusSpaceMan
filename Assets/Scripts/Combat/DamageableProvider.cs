using System;
using UnityEngine;

public class DamageableProvider : MonoBehaviour, IDamageable
{
    [SerializeField][SerializeInterface(typeof(IDamageable))] private MonoBehaviour _mainDamageableMonoBehaviour;

    private Transform _transform;
    private IDamageable _mainDamageable;

    public event Action<Hit, float> BeforeTakeDamage;
    public event Action<Hit, float> AfterTakeDamage;

    public Vector3 Position => _transform.position;
    public Quaternion Rotation => _transform.rotation;
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