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
        _transform = transform;
        _rangedWeapon = (IRangedWeaponReadOnly)_rangedWeaponMonoBehaviour;
    }

    protected override void EnableAddon()
    {
        _rangedWeapon.Initialized += OnInitialized;
    }

    protected override void DisableAddon()
    {
        _rangedWeapon.Initialized -= OnInitialized;
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
        projectile.Transform.rotation = _rangedWeapon.StartPoint.rotation;
        projectile.Transform.position = _rangedWeapon.StartPoint.position;
        projectile.Transform.parent = null;
        projectile.gameObject.SetActive(true);
    }

    protected override int InitilizeCapacity() => _capacity;

    protected override void RefundSpawnObject(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
        projectile.transform.parent = transform;
    }

    private void OnInitialized()
    {
        _prefab = _rangedWeapon.RangedWeaponConfig.ProjectileConfig.Prefab;
        Initilize();
    }

    private void OnDestroyed(Projectile projectile)
    {
        projectile.Destroyed -= OnDestroyed;
        Pool.Refund(projectile);
    }
}