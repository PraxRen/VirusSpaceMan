using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Fighter : MonoBehaviour, IDamageable, IFighterReadOnly
{
    [SerializeField] private Health _health;
    [SerializeField][SerializeInterface(typeof(IChangerWeaponConfig))] private MonoBehaviour _changerWeaponConfigMonoBehaviour;
    [SerializeField][SerializeInterface(typeof(IAttackNotifier))] private MonoBehaviour _attackNotifierMonoBehaviour;
    [SerializeField] private LayerMask _layerMaskDamageable;
    [SerializeField] private LayerMask _layerMaskCollision;
    [SerializeField] private Weapon _weaponDEBUG;

    private IChangerWeaponConfig _changerWeaponConfig;
    private IAttackNotifier _attackNotifier;
    private IDamageable _currentDamageable;
    private Weapon _currentWeapon;
    private Transform _transform;
    private bool _isActivateWeapon;

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

    public bool CanAttack()
    {
        if (IsAttack)
            return false;

        if (_isActivateWeapon == false)
            return false;

        if (_currentWeapon.CanAttack() == false)
            return false;

        return true;
    }

    public void Attack()
    {
        IsAttack = true;
        _attackNotifier.CreateAttack();
    }

    private void OnStartingAttack()
    {

    }

    private void OnRunningDamage()
    {

    }

    private void OnStoppingAttack()
    {
        IsAttack = false;
    }

    private void OnChangedWeaponConfig(WeaponConfig weaponConfig)
    {
        RemoveWeapon();
        _currentWeapon = _weaponDEBUG;
        _currentWeapon.Init(weaponConfig, this);
        ChangedWeapon?.Invoke(_currentWeapon);
    }

    private void RemoveWeapon()
    {
        if (_currentWeapon == null)
            return;

        _currentWeapon.ClearConfig();
        _currentWeapon = null;
        RemovedWeapon?.Invoke();
    }
}