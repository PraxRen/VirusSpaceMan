using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public abstract class Projectile : MonoBehaviour, ISurface
{
    private const float RadiusRaycatsForward = 0.2f;
    private const float DistanceRaycatsForward = 1f;

    private Rigidbody _rigidbody;
    private Collider _collider;
    private IRangedWeaponReadOnly _rangedWeapon;
    private ProjectileConfig _projectileConfig;

    public event Action<Projectile, Collider> Collided;
    public event Action<Projectile> Destroyed;

    public Transform Transform { get; private set; }
    public Attack Attack { get; private set; }
    public float FactorNoise => _projectileConfig.FactorNoise;
    public SurfaceType SurfaceType => _projectileConfig.SurfaceType;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
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

        HandleCollide(collision);
    }

    private void Update()
    {
        bool isCollide = Physics.SphereCast(transform.position, RadiusRaycatsForward, transform.forward, out RaycastHit hit, DistanceRaycatsForward);

        if (isCollide == true && _rangedWeapon.CanCollide(hit.collider) == false)
        {
            Physics.IgnoreCollision(_collider, hit.collider);
        }
    }

    public void Initialize(IRangedWeaponReadOnly rangedWeapon)
    {
        _rangedWeapon = rangedWeapon;
        _projectileConfig = ((RangedWeaponConfig)rangedWeapon.Config).ProjectileConfig;
    }

    public void Shoot(Vector3 direction, Attack attack)
    {
        Attack = attack;
        Transform.forward = direction;
        _rigidbody.AddForce(Transform.forward * _projectileConfig.Speed, ForceMode.Impulse);
        ShootAddon();
    }

    protected void ShootAddon() { }

    protected virtual void EnableAddon() { }

    protected virtual void DisableAddon() { }

    protected virtual void BeforeHandleCollideAddon(Collision collision) { }
    
    protected virtual void AfterHandleCollideAddon(Collision collision) { }

    protected virtual bool CanCollideAddon(Collider collider) => true;

    protected void Destroy()
    {
        _rigidbody.velocity = Vector3.zero;
        Destroyed?.Invoke(this);
    }

    private void HandleCollide(Collision collision)
    {
        BeforeHandleCollideAddon(collision);
        Collided?.Invoke(this, collision.collider);
        AfterHandleCollideAddon(collision);
    }
}