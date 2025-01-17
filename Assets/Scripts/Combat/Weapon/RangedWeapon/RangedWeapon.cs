using UnityEngine;

public abstract class RangedWeapon : Weapon, IRangedWeaponReadOnly
{
    [SerializeField] private SpawnerProjectile _spawnerProjectile;
    [SerializeField] private Transform _startPoint;

    private Projectile _lastProjectile;

    public Transform StartPoint => _startPoint;
    public RangedWeaponConfig RangedWeaponConfig {  get; private set; }


    protected override void ActivateAddon()
    {
        _spawnerProjectile.enabled = true;
    }

    protected override void DeactivateAddon()
    {
        _spawnerProjectile.enabled = false;
    }

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
        projectile.Shoot((targetPosition - Transform.position).normalized, CurrentAttack);
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
        Vector3 targetPosition = Fighter.LookTracker.Position + new Vector3(randomPointInCircle.x, randomPointInCircle.y, 0f);

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
        HandleCollide(collider, projectile.Attack, hitPoint);
    }
}