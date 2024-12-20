using System;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour, IDamageable, IFighterReadOnly, IAction, IModeMoverChanger
{
    [Tooltip("Auto-Activate the weapon when it is changed")][SerializeField] private bool _isAutoActivationWeapon;
    [SerializeField] private Health _health;
    [SerializeField] private StorageWeapon _storageWeapon;
    [SerializeField] private ActionScheduler _actionScheduler;
    [SerializeField] private Rage _rage;
    [Range(0f, 1f)][SerializeField] private float _luckRageAttack;
    [SerializeField][SerializeInterface(typeof(IChangerWeaponConfig))] private MonoBehaviour _changerWeaponConfigMonoBehaviour;
    [SerializeField][SerializeInterface(typeof(IAttackNotifier))] private MonoBehaviour _attackNotifierMonoBehaviour;
    [SerializeField][SerializeInterface(typeof(IReadOnlyTargetTracker))] private MonoBehaviour _lookTrackerMonoBehaviour;
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
    public event Action<Hit, float> BeforeTakeDamage;
    public event Action<Hit, float> AfterTakeDamage;
    public event Action<IModeMoverProvider> AddedModeMover;
    public event Action<IModeMoverProvider> RemovedModeMover;

    public bool IsAttack { get; private set; }
    public IWeaponReadOnly Weapon => _currentWeapon;
    public LayerMask LayerMaskDamageable => _layerMaskDamageable;
    public LayerMask LayerMaskCollision => _layerMaskCollision;
    public Vector3 Position => _transform.position;
    public Quaternion Rotation => _transform.rotation;
    public IReadOnlyCollection<Collider> IgnoreColliders => _ignoreColliders;
    public SurfaceType SurfaceType => _currentDamageable.SurfaceType;
    public float FactorNoise => _currentDamageable.FactorNoise;
    public IReadOnlyTargetTracker LookTracker {  get; private set; }

    private void Awake()
    {
        _transform = transform;
        _changerWeaponConfig = (IChangerWeaponConfig)_changerWeaponConfigMonoBehaviour;
        _attackNotifier = (IAttackNotifier)_attackNotifierMonoBehaviour;
        LookTracker = (IReadOnlyTargetTracker)_lookTrackerMonoBehaviour;
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

    public bool CanTakeDamage() => _currentDamageable.CanTakeDamage();

    public void TakeDamage(Hit hit, float damage) 
    {
        BeforeTakeDamage?.Invoke(hit, damage);
        _currentDamageable.TakeDamage(hit, damage);
        AfterTakeDamage?.Invoke(hit, damage);
    } 

    public bool CanDie(Hit hit, float damage) => _currentDamageable.CanDie(hit, damage);

    public void ActivateWeapon()
    {
        if (_currentWeapon == null)
            return;

        _currentWeapon.Activate();

        if (_currentWeapon.Config is IModeMoverProvider modeMoverProvider)
            AddedModeMover?.Invoke(modeMoverProvider);

        ActivatedWeapon?.Invoke(_currentWeapon);
    }

    public void DeactivateWeapon()
    {
        if (_currentWeapon == null)
            return;

        Cancel();
        _currentWeapon.Deactivate();

        if (_currentWeapon.Config is IModeMoverProvider modeMoverProvider)
            RemovedModeMover?.Invoke(modeMoverProvider);

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
        if (IsAttack == false)
            return;

        _currentWeapon.StopAttack();
        _attackNotifier.CancelAttack();
        _actionScheduler.ClearAction(this);
        IsAttack = false;
    }

    private void OnHited(IDamageable damageable, IWeaponReadOnly weapon, Attack attack, Vector3 hitPoint)
    {
        if (damageable.CanTakeDamage() == false)
            return;

        _rage.AddPoint(attack.RagePoints);
        bool IsRageAttack = SimpleUtils.TryLuck(_luckRageAttack + _rage.Value);
        Hit hit = new Hit(weapon, attack, hitPoint, IsRageAttack);
        damageable.TakeDamage(hit, hit.BaseDamage);
    }

    private void OnChangedWeaponConfig(WeaponConfig weaponConfig)
    {
        RemoveWeapon();
        _currentWeapon = _storageWeapon.GetWeapon(weaponConfig.IdWeapon);
        _currentWeapon.Hited += OnHited;
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
        _currentWeapon.Hited -= OnHited;
        _currentWeapon = null;
        RemovedWeapon?.Invoke();
    }

    public bool CanReach(Transform transform) => _currentDamageable.CanReach(transform);
}