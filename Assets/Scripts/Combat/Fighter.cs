using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Fighter : MonoBehaviour, IDamageable, IFighterReadOnly
{
    [SerializeField] private Health _health;
    [SerializeField] private StorageWeapon _storageWeapon;
    [SerializeField][SerializeInterface(typeof(IChangerWeaponConfig))] private MonoBehaviour _changerWeaponConfigMonoBehaviour;
    [SerializeField][SerializeInterface(typeof(IAttackNotifier))] private MonoBehaviour _attackNotifierMonoBehaviour;
    [SerializeField] private LayerMask _layerMaskDamageable;
    [SerializeField] private LayerMask _layerMaskCollision;
    [SerializeField] private bool _isAutoActivationWeapon;

    private IChangerWeaponConfig _changerWeaponConfig;
    private IAttackNotifier _attackNotifier;
    private IDamageable _currentDamageable;
    private Weapon _currentWeapon;
    private Transform _transform;

    public event Action<IWeaponReadOnly> ChangedWeapon;
    public event Action RemovedWeapon;

    public bool IsAttack { get; private set; }
    public IWeaponReadOnly Weapon => _currentWeapon;
    public LayerMask LayerMaskDamageable => _layerMaskDamageable;
    public Vector3 Position => _transform.position;

    private void Awake()
    {
        _transform = transform;
        _changerWeaponConfig = (IChangerWeaponConfig)_changerWeaponConfigMonoBehaviour;
        _attackNotifier = (IAttackNotifier)_attackNotifierMonoBehaviour;
        _currentDamageable = _health;
    }

    private void OnEnable()
    {
        _changerWeaponConfig.ChangedWeaponConfig += OnChangedWeaponConfig;
        _changerWeaponConfig.RemovedWeaponConfig += RemoveWeapon;
        _attackNotifier.StartingAttack += OnStartingAttack;
        _attackNotifier.RunningDamage += OnRunningDamage;
        _attackNotifier.StoppingAttack += OnStoppingAttack;
    }

    private void OnDisable()
    {
        _changerWeaponConfig.ChangedWeaponConfig -= OnChangedWeaponConfig;
        _changerWeaponConfig.RemovedWeaponConfig -= RemoveWeapon;
        _attackNotifier.StartingAttack -= OnStartingAttack;
        _attackNotifier.RunningDamage -= OnRunningDamage;
        _attackNotifier.StoppingAttack -= OnStoppingAttack;
    }

    public bool CanTakeDamage(IWeaponReadOnly weapon) => _currentDamageable.CanTakeDamage(weapon);

    public void TakeDamage(IWeaponReadOnly weapon, float damage) => _currentDamageable.TakeDamage(weapon, damage);

    public bool CanDie(IWeaponReadOnly weapon, float damage) => _currentDamageable.CanDie(weapon, damage);

    public void ActivateWeapon()
    {
        if (_isAutoActivationWeapon)
            return;

        if (_currentWeapon == null)
            return;

        _currentWeapon.Activate();
    }

    public void DeactivateWeapon()
    {
        if (_isAutoActivationWeapon)
            return;

        if (_currentWeapon == null)
            return;

        _currentWeapon.Deactivate();
    }

    public bool CanAttack()
    {
        if (IsAttack)
            return false;

        if (_currentWeapon == null) 
            return false;

        if (_currentWeapon.CanAttack() == false)
            return false;

        return true;
    }

    public void Attack(IDamageable target)
    {
        Vector3 direction = (target.Position - _transform.position).normalized;
        direction.y = 0f;
        _transform.forward = direction;
        Attack();
    }

    public void Attack()
    {
        IsAttack = true;
        _attackNotifier.CreateAttack();
    }

    private void OnStartingAttack()
    {
        _currentWeapon.StartAttack();
    }

    private void OnRunningDamage()
    {
        _currentWeapon.RunDamage();
    }

    private void OnStoppingAttack()
    {
        _currentWeapon.StopAttack();
        _attackNotifier.CancelAttack();
        IsAttack = false;
    }

    public bool CanHit(Transform target)
    {
        if (target == _transform)
            return false;

        if (SimpleUtils.IsLayerInclud(target.gameObject, _layerMaskCollision) == false)
            return false;

        return true;
    }

    private void OnHited(Transform target)
    {
        Debug.Log(target.name);

        if (target.TryGetComponent(out IDamageable damageable))
        {
            HandleHitDamageable(damageable);
        }
    }

    private void HandleHitDamageable(IDamageable damageable)
    {
        if (damageable.CanTakeDamage(Weapon) == false)
            return;

        damageable.TakeDamage(Weapon, Weapon.Config.Damage);
    }

    private void OnChangedWeaponConfig(WeaponConfig weaponConfig)
    {
        RemoveWeapon();
        _currentWeapon = _storageWeapon.GetWeapon(weaponConfig.IdWeapon);
        _currentWeapon.Hited += OnHited;
        _currentWeapon.Init(weaponConfig, this);

        if (_isAutoActivationWeapon)
            _currentWeapon.Activate();

        ChangedWeapon?.Invoke(_currentWeapon);
    }

    private void RemoveWeapon()
    {
        if (_currentWeapon == null)
            return;

        _currentWeapon.ClearConfig();
        _currentWeapon.Hited -= OnHited;
        _currentWeapon = null;
        RemovedWeapon?.Invoke();
    }


}