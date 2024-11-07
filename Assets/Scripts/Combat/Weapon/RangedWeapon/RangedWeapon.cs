using System;
using UnityEngine;

public abstract class RangedWeapon : Weapon, IRangedWeaponReadOnly
{
    [SerializeField] private SpawnerProjectile _spawnerProjectile;
    [SerializeField] private Transform _startPoint;

    public Transform StartPoint => _startPoint;

    public RangedWeaponConfig RangedWeaponConfig {  get; private set; }

    protected override void InitializeAddon(WeaponConfig config, IFighterReadOnly fighter)
    {
        RangedWeaponConfig = (RangedWeaponConfig)config;
    }

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

    protected override float GetDamageAddon() => RangedWeaponConfig.Damage;

    private void OnCollided(Projectile projectile, Collider collider)
    {
        projectile.Collided -= OnCollided;
        HandleCollide(collider);
    }
}