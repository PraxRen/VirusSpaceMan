using UnityEngine;

public class SpawnerProjectile : Spawner<Projectile>
{
    [SerializeField][SerializeInterface(typeof(IRangedWeaponReadOnly))] private MonoBehaviour _rangedWeaponMonoBehaviour;
    [SerializeField] private int _capacity;

    private Transform _transform;
    private IRangedWeaponReadOnly _rangedWeapon;
    private Projectile _prefab;

    protected override void AwakeAddon()
    {
        if (_transform != null)
            return;

        _transform = transform;
        _rangedWeapon = (IRangedWeaponReadOnly)_rangedWeaponMonoBehaviour;
    }

    protected override void BeforeInitializeAddon()
    {
        if (_transform == null)
        {
            AwakeAddon();
        }

        _prefab = _rangedWeapon.RangedWeaponConfig.ProjectileConfig.Prefab;
    }

    protected override Projectile CreateSpawnObjectAddon()
    {
        Projectile projectile = Instantiate(_prefab, _transform);
        projectile.Initialize(_rangedWeapon);
        projectile.gameObject.SetActive(false);

        return projectile;
    }

    protected override void GetSpawnObject(Projectile projectile)
    {
        projectile.Destroyed += OnDestroyed;
        projectile.gameObject.SetActive(true);
        projectile.Transform.parent = null;
    }

    protected override int InitilizeCapacity() => _capacity;

    protected override void RefundSpawnObject(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
        projectile.Transform.parent = transform;
        projectile.Transform.rotation = _rangedWeapon.StartPoint.rotation;
        projectile.Transform.position = _rangedWeapon.StartPoint.position;
    }

    private void OnDestroyed(Projectile projectile)
    {
        projectile.Destroyed -= OnDestroyed;
        Pool.Refund(projectile);
    }
}