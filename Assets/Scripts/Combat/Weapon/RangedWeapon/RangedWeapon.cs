using UnityEngine;

public abstract class RangedWeapon : Weapon, IRangedWeaponReadOnly
{
    [SerializeField] private SpawnerProjectile _spawnerProjectile;
    [SerializeField] private Transform _startPoint;

    private Projectile _lastProjectile;

    public Transform StartPoint => _startPoint;
    public RangedWeaponConfig RangedWeaponConfig {  get; private set; }

    protected override void InitializeAddon(WeaponConfig config, IFighterReadOnly fighter)
    {
        RangedWeaponConfig = (RangedWeaponConfig)config;
        _spawnerProjectile.Initialize();
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

    protected override void HandleCollidable(ICollidable collidable)
    {
        collidable.HandleCollide(_lastProjectile);
    }

    private void OnCollided(Projectile projectile, Collider collider)
    {
        projectile.Collided -= OnCollided;
        _lastProjectile = projectile;
        HandleCollide(collider);
    }
}