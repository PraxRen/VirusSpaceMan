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
        Vector3 targetPosition = GetTargetPosition();
        projectile.Shoot((targetPosition - Transform.position).normalized);
    }

    protected override float GetDamageAddon() => RangedWeaponConfig.ProjectileConfig.Damage;

    protected override void HandleCollidable(ICollidable collidable)
    {
        collidable.HandleCollide(_lastProjectile);
    }

    private Vector3 GetTargetPosition()
    {
        float adjustedRadius = RangedWeaponConfig.MaxRadiusAccuracy * (1 - RangedWeaponConfig.Accuracy);
        Vector2 randomPointInCircle = Random.insideUnitCircle * adjustedRadius;
        Vector3 targetPosition = Fighter.LookTarget.Position + new Vector3(randomPointInCircle.x, 0, randomPointInCircle.y);

        return targetPosition;
    }

    private void OnCollided(Projectile projectile, Collider collider)
    {
        projectile.Collided -= OnCollided;
        _lastProjectile = projectile;
        Vector3 hitPoint = collider.ClosestPoint(projectile.Transform.position);
#if UNITY_EDITOR
        Debug.DrawLine(collider.GetComponent<Transform>().position, hitPoint, Color.red, 2f);
#endif
        HandleCollide(collider, hitPoint);
    }
}