using System;
using UnityEngine;

public class DamageableProvider : MonoBehaviour, IDamageable
{
    [SerializeField][SerializeInterface(typeof(IDamageable))] private MonoBehaviour _mainDamageableMonoBehaviour;

    private Transform _transform;
    private IDamageable _mainDamageable;

    public event Action<IWeaponReadOnly, Vector3, float> BeforeTakeDamage;
    public event Action<IWeaponReadOnly, Vector3, float> AfterTakeDamage;

    public Vector3 Position => _transform.position;
    public Quaternion Rotation => _transform.rotation;
    public float FactorNoise => _mainDamageable.FactorNoise;
    public SurfaceType SurfaceType => _mainDamageable.SurfaceType;

    private void Awake()
    {
        _transform = transform;
        _mainDamageable = (IDamageable)_mainDamageableMonoBehaviour;
    }

    public bool CanDie(IWeaponReadOnly weapon, float damage) => _mainDamageable.CanDie(weapon, damage);

    public bool CanTakeDamage(IWeaponReadOnly weapon) => _mainDamageable.CanTakeDamage(weapon);

    public void TakeDamage(IWeaponReadOnly weapon, Vector3 hitPoint, float damage) 
    {
        BeforeTakeDamage?.Invoke(weapon, hitPoint, damage);
        _mainDamageable.TakeDamage(weapon, hitPoint, damage);
        AfterTakeDamage?.Invoke(weapon, hitPoint, damage);
    } 
}