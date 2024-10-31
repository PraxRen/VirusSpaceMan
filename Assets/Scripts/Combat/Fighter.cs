using System;
using UnityEngine;

public class Fighter : MonoBehaviour, IDamageable, IFighterReadOnly
{
    [SerializeField][SerializeInterface(typeof(IStorageFighter))] private MonoBehaviour _storageFighterMonoBehaviour;
    [SerializeField][SerializeInterface(typeof(IAttackNotifier))] private MonoBehaviour _attackNotifierMonoBehaviour;
    [SerializeField] private LayerMask _layerMaskDamageable;
    [SerializeField] private LayerMask _layerMaskCollision;

    private IStorageFighter _storageFighter;
    private IAttackNotifier _attackNotifier;
    private IDamageable _currentDamageable;
    private Weapon _currentWeapon;
    private bool _isActivateWeapon;

    public bool IsAttack { get; private set; }

    private void Awake()
    {
        _storageFighter = (IStorageFighter)_storageFighterMonoBehaviour;
        _attackNotifier = (IAttackNotifier)_attackNotifierMonoBehaviour;
    }

    private void OnEnable()
    {
        _storageFighter.ChangedDamageable += OnChangedDamageable;
        _storageFighter.ChangedWeapon += OnChangedWeapon;
        _attackNotifier.StartingAttack += OnStartingAttack;
        _attackNotifier.RunningDamage += OnRunningDamage;
        _attackNotifier.StoppingAttack += OnStoppingAttack;
    }

    private void OnDisable()
    {
        _storageFighter.ChangedDamageable -= OnChangedDamageable;
        _storageFighter.ChangedWeapon -= OnChangedWeapon;
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

    private void OnChangedDamageable(IDamageable damageable)
    {
        _currentDamageable = damageable;
    }

    private void OnChangedWeapon(Weapon weapon)
    {
        _currentWeapon = weapon;
    }
}