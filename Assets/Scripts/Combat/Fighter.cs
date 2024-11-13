using System;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour, IDamageable, IFighterReadOnly, IAction
{
    [Tooltip("Auto-Activate the weapon when it is changed")][SerializeField] private bool _isAutoActivationWeapon;
    [SerializeField] private Health _health;
    [SerializeField] private StorageWeapon _storageWeapon;
    [SerializeField] private ActionScheduler _actionScheduler;
    [SerializeField][SerializeInterface(typeof(IChangerWeaponConfig))] private MonoBehaviour _changerWeaponConfigMonoBehaviour;
    [SerializeField][SerializeInterface(typeof(IAttackNotifier))] private MonoBehaviour _attackNotifierMonoBehaviour;
    [SerializeField][SerializeInterface(typeof(IReadOnlyLookTarget))] private MonoBehaviour _lookTargetMonoBehaviour;
    [SerializeField] private LayerMask _layerMaskDamageable;
    [SerializeField] private LayerMask _layerMaskCollision;
    [SerializeField] private Collider[] _ignoreColliders;

    private IChangerWeaponConfig _changerWeaponConfig;
    private IAttackNotifier _attackNotifier;
    private IDamageable _currentDamageable;
    private Weapon _currentWeapon;
    private Transform _transform;

    public event Action<IWeaponReadOnly> ChangedWeapon;
    public event Action<IWeaponReadOnly> ActivatedWeapon;
    public event Action<IWeaponReadOnly> DeactivatedWeapon;
    public event Action RemovedWeapon;
    public event Action<IWeaponReadOnly, float> BeforeTakeDamage;
    public event Action<IWeaponReadOnly, float> AfterTakeDamage;

    public bool IsAttack { get; private set; }
    public IWeaponReadOnly Weapon => _currentWeapon;
    public LayerMask LayerMaskDamageable => _layerMaskDamageable;
    public LayerMask LayerMaskCollision => _layerMaskCollision;
    public Vector3 Position => _transform.position;
    public IReadOnlyCollection<Collider> IgnoreColliders => _ignoreColliders;
    public SurfaceType SurfaceType => _currentDamageable.SurfaceType;
    public float FactorNoise => _currentDamageable.FactorNoise;
    public IReadOnlyLookTarget LookTarget {  get; private set; }

    private void Awake()
    {
        _transform = transform;
        _changerWeaponConfig = (IChangerWeaponConfig)_changerWeaponConfigMonoBehaviour;
        _attackNotifier = (IAttackNotifier)_attackNotifierMonoBehaviour;
        LookTarget = (IReadOnlyLookTarget)_lookTargetMonoBehaviour;
        _currentDamageable = _health;
    }

    private void OnEnable()
    {
        _changerWeaponConfig.ChangedWeaponConfig += OnChangedWeaponConfig;
        _changerWeaponConfig.RemovedWeaponConfig += RemoveWeapon;
        _attackNotifier.StartingAttack += OnStartingAttack;
        _attackNotifier.RunningDamage += OnRunningDamage;
        _attackNotifier.StoppingAttack += StopAttack;
    }

    private void OnDisable()
    {
        _changerWeaponConfig.ChangedWeaponConfig -= OnChangedWeaponConfig;
        _changerWeaponConfig.RemovedWeaponConfig -= RemoveWeapon;
        _attackNotifier.StartingAttack -= OnStartingAttack;
        _attackNotifier.RunningDamage -= OnRunningDamage;
        _attackNotifier.StoppingAttack -= StopAttack;
    }

    public void Cancel() => StopAttack();

    public bool CanTakeDamage(IWeaponReadOnly weapon) => _currentDamageable.CanTakeDamage(weapon);

    public void TakeDamage(IWeaponReadOnly weapon, float damage) 
    {
        BeforeTakeDamage?.Invoke(weapon, damage);
        _currentDamageable.TakeDamage(weapon, damage);
        AfterTakeDamage?.Invoke(weapon, damage);
    } 

    public bool CanDie(IWeaponReadOnly weapon, float damage) => _currentDamageable.CanDie(weapon, damage);

    public void ActivateWeapon()
    {
        if (_currentWeapon == null)
            return;

        _currentWeapon.Activate();
        ActivatedWeapon?.Invoke(_currentWeapon);
    }

    public void DeactivateWeapon()
    {
        if (_currentWeapon == null)
            return;

        _currentWeapon.Deactivate();
        DeactivatedWeapon?.Invoke(_currentWeapon);
    }

    public bool CanAttack()
    {
        if (IsAttack)
            return false;

        if (_currentWeapon == null) 
            return false;

        if (_currentWeapon.CanAttack() == false)
            return false;

        if(_attackNotifier.CanCreateAttack() == false)
            return false;

        if (_actionScheduler.CanStartAction(this) == false)
            return false;

        return true;
    }

    public void Attack()
    {
        IsAttack = true;
        _actionScheduler.StartAction(this);
        _actionScheduler.SetBlock(this);
        _currentWeapon.UpdateIndexAttack();
        _attackNotifier.CreateAttack();
    }

    private void OnStartingAttack()
    {
        if (IsAttack == false)
            return;

        _currentWeapon.StartAttack();
    }

    private void OnRunningDamage()
    {
        if (IsAttack == false)
            return;

        _currentWeapon.RunDamage();
    }

    private void StopAttack()
    {
        _currentWeapon.StopAttack();
        _attackNotifier.CancelAttack();
        _actionScheduler.ClearAction(this);
        IsAttack = false;
    }

    private void Hit(IDamageable damageable)
    {
        if (damageable.CanTakeDamage(Weapon) == false)
            return;

        damageable.TakeDamage(Weapon, Weapon.GetDamage());
    }

    private void OnChangedWeaponConfig(WeaponConfig weaponConfig)
    {
        RemoveWeapon();
        _currentWeapon = _storageWeapon.GetWeapon(weaponConfig.IdWeapon);
        _currentWeapon.Hited += Hit;
        _currentWeapon.Initialize(weaponConfig, this);

        if (_isAutoActivationWeapon)
            ActivateWeapon();

        ChangedWeapon?.Invoke(_currentWeapon);
    }

    private void RemoveWeapon()
    {
        if (_currentWeapon == null)
            return;

        DeactivatedWeapon?.Invoke(_currentWeapon);
        _currentWeapon.ClearConfig();
        _currentWeapon.Hited -= Hit;
        _currentWeapon = null;
        RemovedWeapon?.Invoke();
    }
}