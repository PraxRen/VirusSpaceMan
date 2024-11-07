using System;
using UnityEngine;

public abstract class RangedWeapon : Weapon, IRangedWeaponReadOnly
{
    [SerializeField] private SpawnerProjectile _spawnerProjectile;
    [SerializeField] private Transform _startPoint;

    public Projectile ProjectilePrefab => throw new NotImplementedException();

    public Transform StartPoint => _startPoint;

    protected override bool CanAttackAddon()
    {
        return _spawnerProjectile.CanSpawn();
    }

    protected override void RunDamageAddon()
    {
        Projectile projectile = _spawnerProjectile.Spawn();
        projectile.Collided += OnCollided;
        projectile.Shoot();
    }

    protected override float GetDamageAddon()
    {
        return 0f;
    }

    private void OnCollided(Projectile projectile, Collider collider)
    {
        projectile.Collided -= OnCollided;
        HandleCollide(collider);
    }
}