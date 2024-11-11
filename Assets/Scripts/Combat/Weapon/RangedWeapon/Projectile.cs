using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Projectile : MonoBehaviour, ISurface
{
    private Rigidbody _rigidbody;
    private IRangedWeaponReadOnly _rangedWeapon;
    private ProjectileConfig _projectileConfig;

    public event Action<Projectile, Collider> Collided;
    public event Action<Projectile> Destroyed;

    public Transform Transform { get; private set; }
    public float FactorNoise => _projectileConfig.FactorNoise;
    public SurfaceType SurfaceType => _projectileConfig.SurfaceType;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Transform = transform;
    }

    private void OnEnable()
    {
        EnableAddon();
    }

    private void OnDisable()
    {
        DisableAddon();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider targetCollider = collision.collider;

        if (CanCollideAddon(targetCollider) == false)
            return;

        if (_rangedWeapon.CanCollide(targetCollider) == false)
            return;

        HandleCollide(targetCollider);
    }

    public void Initialize(IRangedWeaponReadOnly rangedWeapon)
    {
        _rangedWeapon = rangedWeapon;
        _projectileConfig = ((RangedWeaponConfig)rangedWeapon.Config).ProjectileConfig;
    }

    public void Shoot(Vector3 direction)
    {
        Transform.forward = direction;
        _rigidbody.AddForce(Transform.forward * _projectileConfig.Speed, ForceMode.Impulse);
        ShootAddon();
    }

    protected void ShootAddon() { }

    protected virtual void EnableAddon() { }

    protected virtual void DisableAddon() { }

    protected virtual void BeforeHandleCollideAddon(Collider collider) { }
    
    protected virtual void AfterHandleCollideAddon() { }

    protected virtual bool CanCollideAddon(Collider collider) => true;

    protected void Destroy()
    {
        _rigidbody.velocity = Vector3.zero;
        Destroyed?.Invoke(this);
    }

    private void HandleCollide(Collider collider)
    {
        BeforeHandleCollideAddon(collider);
        Collided?.Invoke(this, collider);
        AfterHandleCollideAddon();
    }
}